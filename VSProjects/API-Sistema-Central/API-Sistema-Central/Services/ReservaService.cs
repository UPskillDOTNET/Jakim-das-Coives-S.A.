using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API_Sistema_Central.DTOs;
using API_Sistema_Central.Models;
using API_Sistema_Central.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API_Sistema_Central.Services
{
    public interface IReservaService
    {
        public Task<ActionResult<IEnumerable<LugarDTO>>> FindAvailableAsync(string freguesiaNome, DateTime inicio, DateTime fim);
        public Task<ActionResult<IEnumerable<DetalheReservaDTO>>> GetByNifAsync(string nif);
        public Task<DetalheReservaDTO> GetByIdAsync(int id);
        public Task<Reserva> PostAsync(ReservaDTO reservaDTO);
        public Task DeleteAsync(int id);
    }

    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _repository;
        private readonly IParqueRepository _parqueRepository;
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly IEmailService _emailService;
        private readonly UserManager<Utilizador> _userManager;
        private readonly IPagamentoService _pagamentoService;

        public ReservaService(IReservaRepository repository, IParqueRepository parqueRepository, ITransacaoRepository transacaoRepository, UserManager<Utilizador> userManager, IPagamentoService pagamentoService, IEmailService emailService)
        {
            _repository = repository;
            _parqueRepository = parqueRepository;
            _transacaoRepository = transacaoRepository;
            _userManager = userManager;
            _emailService = emailService;
            _pagamentoService = pagamentoService;
        }

        public async Task<ActionResult<IEnumerable<LugarDTO>>> FindAvailableAsync(string freguesiaNome, DateTime inicio, DateTime fim)
        {
            var listaLugares = new List<LugarDTO>();
            var listaParques = _parqueRepository.GetAllAsync().Result;
            foreach (Parque parque in listaParques)
            {
                var f = GetFreguesiaByNome(freguesiaNome, parque.ApiUrl).Result;
                if (f != null)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "aee2fb676a2e4b25a819af617eb64174");
                        string endpoint1 = parque.ApiUrl + "api/lugares/disponibilidade/" + f.Id + "/" + inicio.ToString("yyyy-MM-ddTHH:mm:ss") + "/" + fim.ToString("yyyy-MM-ddTHH:mm:ss");
                        var response1 = await client.GetAsync(endpoint1);
                        response1.EnsureSuccessStatusCode();
                        List<LugarDTO> temp = await response1.Content.ReadAsAsync<List<LugarDTO>>();
                        foreach (LugarDTO l in temp)
                        {
                            l.ParqueIdSC = parque.Id;
                            l.NomeParque = await GetParqueNomeByID(l.ParqueId, parque.ApiUrl);
                            l.Inicio = inicio;
                            l.Fim = fim;
                            if (l.NifProprietario == null)
                            {
                                l.NifProprietario = "999999999";
                            }
                            listaLugares.Add(l);
                        }
                    }
                };
            }
            if (!listaLugares.Any())
            {
                throw new Exception("Não existem lugares disponíveis para o local e período de tempo escolhidos.");
            }
            return listaLugares;
        }

        public async Task<ActionResult<IEnumerable<DetalheReservaDTO>>> GetByNifAsync(string nif)
        {
            var u = await _userManager.FindByIdAsync(nif);
            if (u == null)
            {
                throw new Exception("O utilizador não existe.");
            }
            var temp = await _repository.GetAllAsync();
            var l = temp.Where(t => t.NifUtilizador == nif);
            var lista = new List<DetalheReservaDTO>();
            if (!l.Any())
            {
                return lista;
            }
            foreach (Reserva r in l)
            {
                var d = await CreateDetalheReservaDTO(r);
                lista.Add(d);
            }
            return lista;
        }

        public async Task<DetalheReservaDTO> GetByIdAsync(int id)
        {
            var r = await _repository.GetByIdAsync(id);
            if (r == null)
            {
                throw new Exception("A reserva solicitada não existe.");
            }
            var detalhe = CreateDetalheReservaDTO(r).Result;
            return detalhe;
        }

        public async Task<Reserva> PostAsync(ReservaDTO reservaDTO)
        {
            await ValidarReservaDTO(reservaDTO);

            Reserva reserva = new Reserva { NifUtilizador = reservaDTO.NifComprador, ParqueId = reservaDTO.ParqueIdSC };
            Parque p = await _parqueRepository.GetByIdAsync(reservaDTO.ParqueIdSC);
            LugarDTO l = GetLugarParqueByID(reservaDTO.LugarId, p.ApiUrl).Result;

            //Calcular o custo da Reserva
            var h = (reservaDTO.Fim - reservaDTO.Inicio).TotalHours;
            reserva.Custo = Math.Round(h * l.Preco, 2);
            if (reserva.Custo <= 0)
            {
                throw new Exception("O custo da reserva é inválido");
            }

            //Fazer pagamento da reserva
            PagamentoDTO payDTO = new PagamentoDTO { NifPagador = reservaDTO.NifComprador, NifRecipiente = reservaDTO.NifVendedor, MetodoId = reservaDTO.MetodoId, Valor = reserva.Custo };
            await _pagamentoService.Pay(payDTO);

            //Registar a transacao do pagamento da reserva
            Transacao t = await _transacaoRepository.PostAsync(new Transacao { NifPagador = reservaDTO.NifComprador, NifRecipiente = reservaDTO.NifVendedor, Valor = reserva.Custo, MetodoId = reservaDTO.MetodoId, DataHora = DateTime.UtcNow, Tipo = Tipo.Reserva });
            reserva.TransacaoId = t.Id;
            if (t == null)
            {
                await _pagamentoService.Reembolso(new Transacao { NifPagador = reservaDTO.NifComprador, NifRecipiente = reservaDTO.NifVendedor, Valor = reserva.Custo });
                throw new Exception("A transação não foi registada com sucesso.");
            }

            //Reservar o lugar na API-Parque
            try
            {
                var reservaParque = await PostReservaInParqueAPIAsync(reservaDTO, p.ApiUrl);
                reserva.ReservaParqueId = reservaParque.Id;
            }
            catch (Exception)
            {
                await _pagamentoService.Reembolso(t);
                await _transacaoRepository.PostAsync(new Transacao { MetodoId = 4, NifPagador = t.NifRecipiente, NifRecipiente = t.NifPagador, Valor = t.Valor, DataHora = DateTime.UtcNow, Tipo = Tipo.Reembolso });
                throw new Exception("A reserva no parque de destino falhou.");
            }

            //Enviar email de confirmacao
            try
            {
                QRCodeDTO qr = QRCodeDTOAsync(reservaDTO, reserva.ReservaParqueId, l, p.ApiUrl).Result;
                if (p.ApiUrl == "https://jakim-api-management.azure-api.net/sub-alugueres/")
                {
                    var rs = _repository.GetByIdAsync(reservaDTO.ReservaSistemaCentralId).Result;
                    _emailService.EnviarEmailSubAluguer(qr, rs.ReservaParqueId);
                }
                else
                {
                    _emailService.EnviarEmailReserva(qr);
                }
            }
            catch (Exception)
            {
                await _pagamentoService.Reembolso(t);
                await _transacaoRepository.PostAsync(new Transacao { MetodoId = 4, NifPagador = t.NifRecipiente, NifRecipiente = t.NifPagador, Valor = t.Valor, DataHora = DateTime.UtcNow, Tipo = Tipo.Reembolso });
                await DeleteReservaInParqueAPIAsync(reserva.ParqueId, reserva.ReservaParqueId);
                throw new Exception("O envio do email de confirmação falhou.");
            }

            //Reservar no Sistema Central
            try
            {
                return await _repository.PostAsync(reserva);
            }
            catch
            {
                await _pagamentoService.Reembolso(t);
                await _transacaoRepository.PostAsync(new Transacao { MetodoId = 4, NifPagador = t.NifRecipiente, NifRecipiente = t.NifPagador, Valor = t.Valor, DataHora = DateTime.UtcNow, Tipo = Tipo.Reembolso });
                await DeleteReservaInParqueAPIAsync(reserva.ParqueId, reserva.ReservaParqueId);
                Utilizador utilizador = await _userManager.FindByIdAsync(reserva.NifUtilizador);
                _emailService.EnviarEmailCancelamento(utilizador.Nome, reserva.ReservaParqueId, utilizador.Email);
                throw new Exception("A reserva no Sistema Central falhou.");
            }
        }

        public async Task DeleteAsync(int id)
        {
            if (IsSubAlugado(id).Result)
            {
                throw new Exception("Proibido: a reserva está sub-alugada.");
            }
            Reserva reserva = await _repository.GetByIdAsync(id);
            if (reserva == null)
            {
                throw new Exception("A reserva não existe.");
            }
            Transacao t = await _transacaoRepository.GetByIdAsync(reserva.TransacaoId);
            if (t == null)
            {
                throw new Exception("A transação não existe.");
            }
            DetalheReservaDTO dr = await GetByIdAsync(id);
            if (dr.Inicio < DateTime.Now)
            {
                throw new Exception("A hora de início desta reserva já foi ultrapassada.");
            }

            //Enviar email de cancelamento
            try
            {
                Utilizador utilizador = await _userManager.FindByIdAsync(reserva.NifUtilizador);
                _emailService.EnviarEmailCancelamento(utilizador.Nome, reserva.ReservaParqueId, utilizador.Email);
            }
            catch (Exception)
            {
                throw new Exception("O envio do email de cancelamento falhou.");
            }

            //Registar a transacao do reembolso
            Transacao tReembolso;
            try
            {
                tReembolso = await _transacaoRepository.PostAsync(new Transacao { MetodoId = 4, NifPagador = t.NifRecipiente, NifRecipiente = t.NifPagador, Valor = t.Valor, DataHora = DateTime.UtcNow, Tipo = Tipo.Reembolso });
            }
            catch
            {
                throw new Exception("O registo da transação referente ao reembolso falhou.");
            }

            //Reembolsar a carteira do utilizador
            try
            {
                await _pagamentoService.Reembolso(t);
            }
            catch (Exception)
            {
                await _transacaoRepository.DeleteAsync(tReembolso.Id);
                throw new Exception("O reembolso falhou.");
            }

            //Apagar a reserva no sistema central
            try
            {
                await _repository.DeleteAsync(id);
            }
            catch (Exception)
            {
                await _pagamentoService.Reembolso(tReembolso);
                await _transacaoRepository.DeleteAsync(tReembolso.Id);
                throw new Exception("O cancelamento da reserva falhou.");
            }

            //Apagar a reserva no parque
            try
            {
                await DeleteReservaInParqueAPIAsync(reserva.ParqueId, reserva.ReservaParqueId);
            }
            catch (Exception)
            {
                await _pagamentoService.Reembolso(tReembolso);
                await _transacaoRepository.DeleteAsync(tReembolso.Id);
                await _repository.PostAsync(reserva);
                throw new Exception("O cancelamento no parque de destino falhou.");
            }
        }

        private async Task<QRCodeDTO> QRCodeDTOAsync(ReservaDTO reservaDTO, int reservaParqueId, LugarDTO l, string url)
        {
            Utilizador utilizador = await _userManager.FindByIdAsync(reservaDTO.NifComprador);
            var f = GetFreguesiaNomeByParqueID(l.ParqueId, url).Result;
            var p = GetParqueNomeByID(l.ParqueId, url).Result;
            QRCodeDTO qr = new QRCodeDTO { NomeUtilizador = utilizador.Nome, Email = utilizador.Email, ReservaParqueId = reservaParqueId, Inicio = reservaDTO.Inicio, Fim = reservaDTO.Fim, NomeFreguesia = f, NomeParque = p, NumeroLugar = l.Numero, Fila = l.Fila, Andar = l.Andar };
            return qr;
        }
        private static async Task<ReservaAPIParqueDTO> PostReservaInParqueAPIAsync(ReservaDTO reservaDTO, string url)
        {
            try
            {
                ReservaAPIParqueDTO r = new ReservaAPIParqueDTO { NifCliente = 999999999, LugarId = reservaDTO.LugarId, Inicio = reservaDTO.Inicio, Fim = reservaDTO.Fim };
                ReservaAPIParqueDTO r2;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "aee2fb676a2e4b25a819af617eb64174");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(r), Encoding.UTF8, "application/json");
                    string endpoint = url + "api/reservas";
                    var response = await client.PostAsync(endpoint, content);
                    response.EnsureSuccessStatusCode();
                    r2 = await response.Content.ReadAsAsync<ReservaAPIParqueDTO>();
                }
                return r2;
            }
            catch (Exception)
            {
                throw new Exception("A reserva no parque de destino falhou.");
            }
        }
        private async Task DeleteReservaInParqueAPIAsync(int parqueId, int reservaParqueId)
        {
            try
            {
                Parque parque = await _parqueRepository.GetByIdAsync(parqueId);
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "aee2fb676a2e4b25a819af617eb64174");
                    string endpoint1 = parque.ApiUrl + "api/reservas/" + reservaParqueId;
                    var response1 = await client.DeleteAsync(endpoint1);
                    response1.EnsureSuccessStatusCode();
                }
            }
            catch (Exception)
            {
                throw new Exception("O cancelamento no parque de destino falhou.");
            }
        }
        private static async Task<string> GetFreguesiaNomeByParqueID(int id, string url)
        {
            try
            {
                FreguesiaDTO f;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "aee2fb676a2e4b25a819af617eb64174");
                    string endpoint1 = url + "api/parques/" + id;
                    var response1 = await client.GetAsync(endpoint1);
                    response1.EnsureSuccessStatusCode();
                    ParqueDTO p = await response1.Content.ReadAsAsync<ParqueDTO>();

                    string endpoint2 = url + "api/freguesias/" + p.FreguesiaId;
                    var response2 = await client.GetAsync(endpoint2);
                    response2.EnsureSuccessStatusCode();
                    f = await response2.Content.ReadAsAsync<FreguesiaDTO>();
                }
                return f.Nome;
            }
            catch (Exception)
            {
                throw new Exception("GetFreguesiaNomeByParqueID() falhou.");
            }
        }
        private static async Task<string> GetParqueNomeByID(int id, string url)
        {
            try
            {
                ParqueDTO p;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "aee2fb676a2e4b25a819af617eb64174");
                    string endpoint1 = url + "api/parques/" + id;
                    var response1 = await client.GetAsync(endpoint1);
                    response1.EnsureSuccessStatusCode();
                    p = await response1.Content.ReadAsAsync<ParqueDTO>();
                }
                return p.Rua;
            }
            catch (Exception)
            {
                throw new Exception("GetParqueNomeByID() falhou.");
            }
        }
        private static async Task<LugarDTO> GetLugarParqueByID(int id, string url)
        {
            try
            {
                LugarDTO l;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "aee2fb676a2e4b25a819af617eb64174");
                    string endpoint1 = url + "api/lugares/" + id;
                    var response1 = await client.GetAsync(endpoint1);
                    response1.EnsureSuccessStatusCode();
                    l = await response1.Content.ReadAsAsync<LugarDTO>();
                }
                return l;
            }
            catch (Exception)
            {
                throw new Exception("O lugar não existe no parque de destino.");
            }
        }
        private static async Task<ReservaAPIParqueDTO> GetReservaParqueByID(int id, string url)
        {
            try
            {
                ReservaAPIParqueDTO r;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "aee2fb676a2e4b25a819af617eb64174");
                    string endpoint1 = url + "api/reservas/" + id;
                    var response1 = await client.GetAsync(endpoint1);
                    response1.EnsureSuccessStatusCode();
                    r = await response1.Content.ReadAsAsync<ReservaAPIParqueDTO>();
                }
                return r;
            }
            catch (Exception)
            {
                throw new Exception("A reserva não existe no parque de destino.");
            }
        }
        private static async Task<FreguesiaDTO> GetFreguesiaByNome(string nome, string url)
        {
            FreguesiaDTO f;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "aee2fb676a2e4b25a819af617eb64174");
                var listaFreguesias = new List<FreguesiaDTO>();
                string endpoint1 = url + "api/freguesias";
                var response1 = await client.GetAsync(endpoint1);
                response1.EnsureSuccessStatusCode();
                listaFreguesias = await response1.Content.ReadAsAsync<List<FreguesiaDTO>>();
                f = listaFreguesias.Find(z => z.Nome == nome);
            }
            return f;
        }
        private static async Task<bool> IsSubAlugado(int reservaId)
        {
            try
            {
                bool b;
                SubAluguerDTO l;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "aee2fb676a2e4b25a819af617eb64174");
                    var listaSubAlugueres = new List<SubAluguerDTO>();
                    string endpoint1 = "https://jakim-api-management.azure-api.net/sub-alugueres/api/lugares";
                    var response1 = await client.GetAsync(endpoint1);
                    response1.EnsureSuccessStatusCode();
                    listaSubAlugueres = await response1.Content.ReadAsAsync<List<SubAluguerDTO>>();
                    l = listaSubAlugueres.Find(z => z.ReservaSistemaCentralId == reservaId);
                }
                if (l != null) { b = true; }
                else { b = false; }
                return b;
            }
            catch (Exception)
            {
                throw new Exception("IsSubAlugado() falhou.");
            }
        }
        private async Task<DetalheReservaDTO> CreateDetalheReservaDTO(Reserva r)
        {
            Parque parque = await _parqueRepository.GetByIdAsync(r.ParqueId);
            ReservaAPIParqueDTO rp = await GetReservaParqueByID(r.ReservaParqueId, parque.ApiUrl);
            LugarDTO l = await GetLugarParqueByID(rp.LugarId, parque.ApiUrl);
            string p = await GetParqueNomeByID(l.ParqueId, parque.ApiUrl);
            string f = await GetFreguesiaNomeByParqueID(l.ParqueId, parque.ApiUrl);
            bool b = await IsSubAlugado(r.Id);
            DetalheReservaDTO detalhe = new DetalheReservaDTO { NifProprietario = r.NifUtilizador, ReservaId = r.Id, ReservaParqueId = r.ReservaParqueId, Custo = r.Custo, Inicio = rp.Inicio, Fim = rp.Fim, Andar = l.Andar, Fila = l.Fila, NumeroLugar = l.Numero, NomeParque = p, NomeFreguesia = f, IsSubAlugado = b };
            return detalhe;
        }
        private static async Task<SubAluguerDTO> GetLugarSubAluguerByID(int id)
        {
            try
            {
                SubAluguerDTO l;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "aee2fb676a2e4b25a819af617eb64174");
                    string endpoint1 = "https://jakim-api-management.azure-api.net/sub-alugueres/api/lugares/" + id;
                    var response1 = await client.GetAsync(endpoint1);
                    response1.EnsureSuccessStatusCode();
                    l = await response1.Content.ReadAsAsync<SubAluguerDTO>();
                }
                return l;
            }
            catch (Exception)
            {
                throw new Exception("O lugar não existe no parque de sub-aluguer.");
            }
        }
        private async Task ValidarReservaDTO(ReservaDTO reservaDTO)
        {
            if (reservaDTO.Fim < reservaDTO.Inicio)
            {
                throw new Exception("A data e hora de fim não pode ser anterior à data e hora de início.");
            }
            if (reservaDTO.Fim < DateTime.Now)
            {
                throw new Exception("A data e hora de fim não pode ser anterior à data e hora actual.");
            }
            if ((reservaDTO.ReservaSistemaCentralId == 0 && reservaDTO.ParqueIdSC == 5) || (reservaDTO.ReservaSistemaCentralId != 0 && reservaDTO.ParqueIdSC != 5))
            {
                throw new Exception("ReservaSistemaCentralId ou ParqueIdSC incoerentes.");
            }
            if ((reservaDTO.ReservaSistemaCentralId != 0 && reservaDTO.NifVendedor == "999999999") || (reservaDTO.ReservaSistemaCentralId == 0 && reservaDTO.NifVendedor != "999999999"))
            {
                throw new Exception("ReservaSistemaCentralId ou NifVendedor incoerentes.");
            }
            if (reservaDTO.ReservaSistemaCentralId != 0)
            {
                Reserva r = await _repository.GetByIdAsync(reservaDTO.ReservaSistemaCentralId);
                if (r == null)
                {
                    throw new Exception("A reserva para sub-aluguer não existe.");
                }
                if (r.NifUtilizador != reservaDTO.NifVendedor)
                {
                    throw new Exception("Este lugar não pertence ao vendedor.");
                }
                SubAluguerDTO l = await GetLugarSubAluguerByID(reservaDTO.LugarId);
                if (reservaDTO.Inicio < l.Inicio || reservaDTO.Fim > l.Fim)
                {
                    throw new Exception("Início ou fim de sub-aluguer inválido.");
                }
            }
        }
    }
}