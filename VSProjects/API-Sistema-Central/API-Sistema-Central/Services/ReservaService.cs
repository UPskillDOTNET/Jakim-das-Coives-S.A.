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
            foreach (Parque parque in listaParques.Value)
            {
                var f = GetFreguesiaByNome(freguesiaNome, parque.ApiUrl).Result;
                if (f != null)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string endpoint1 = parque.ApiUrl + "api/lugares/disponibilidade/" + f.Id + "/" + inicio.ToString("yyyy-MM-ddTHH:mm:ss") + " / " + fim.ToString("yyyy-MM-ddTHH:mm:ss");
                        var response1 = await client.GetAsync(endpoint1);
                        response1.EnsureSuccessStatusCode();
                        List<LugarDTO> temp = await response1.Content.ReadAsAsync<List<LugarDTO>>();
                        foreach (LugarDTO l in temp)
                        {
                            l.ParqueIdSC = parque.Id;
                            l.ApiUrl = parque.ApiUrl;
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

        public async Task<ActionResult<IEnumerable<Reserva>>> GetByNifAsync(string nif)
        {
            var temp = await _repository.GetAllAsync();
            var lista = temp.Value.Where(t => t.NifUtilizador == nif);
            if (!lista.Any())
            {
                throw new Exception("Não existem reservas associadas a este NIF.");
            }
            return lista.ToList();
        }

        public async Task<Reserva> GetByIdAsync(int id)
        {
            var reserva = await _repository.GetByIdAsync(id);
            if (reserva == null)
            {
                throw new Exception("A reserva solicitada não existe.");
            }
            return reserva;
        }

        public async Task<Reserva> PostAsync(ReservaDTO reservaDTO)
        {
            Reserva reserva = new Reserva { NifUtilizador = reservaDTO.NifUtilizador, ParqueId = reservaDTO.ParqueId };

            if (reservaDTO.Fim < reservaDTO.Inicio)
            {
                throw new Exception("A data e hora de fim não pode ser anterior à data e hora de início.");
            }

            //Calcular o custo da Reserva
            var h = (reservaDTO.Fim - reservaDTO.Inicio).TotalHours;
            reserva.Custo = Math.Round(h * reservaDTO.Preco, 2);
            if (reserva.Custo < 0)
            {
                throw new Exception("O custo da reserva é inválido");
            }

            //Fazer pagamento da reserva
            PagamentoDTO payDTO = new PagamentoDTO { NifPagador = reservaDTO.NifUtilizador, NifRecipiente = reservaDTO.NifVendedor, MetodoId = reservaDTO.MetodoId, Valor = reserva.Custo };
            await _pagamentoService.Pay(payDTO);

            //Registar a transacao do pagamento da reserva
            Transacao t = await _transacaoRepository.PostAsync(new Transacao { NifPagador = reservaDTO.NifUtilizador, NifRecipiente = reservaDTO.NifVendedor, Valor = reserva.Custo, MetodoId = reservaDTO.MetodoId, DataHora = DateTime.UtcNow });
            reserva.TransacaoId = t.Id;
            if (t == null)
            {
                await _pagamentoService.Reembolso(new Transacao { NifPagador = reservaDTO.NifUtilizador, NifRecipiente = reservaDTO.NifVendedor, Valor = reserva.Custo});
                throw new Exception("A transação não foi registada com sucesso.");
            }

            //Reservar o lugar na API-Parque
            var reservaParque = await PostReservaInParqueAPIAsync(reservaDTO);
            reserva.ReservaParqueId = reservaParque.Id;
            if (reserva.ReservaParqueId == 0)
            {
                await _pagamentoService.Reembolso(t);
                await _transacaoRepository.PostAsync(new Transacao { MetodoId = t.MetodoId, NifPagador = t.NifRecipiente, NifRecipiente = t.NifPagador, Valor = t.Valor, DataHora = DateTime.UtcNow });
                throw new Exception("A reserva no parque de destino falhou.");
            }

            //Enviar email de confirmacao
            QRCodeDTO qr = QRCodeDTOAsync(reservaDTO, reserva.ReservaParqueId).Result;
            try
            {
                _emailService.EnviarEmailReserva(qr);
            }
            catch (Exception)
            {
                await _pagamentoService.Reembolso(t);
                await _transacaoRepository.PostAsync(new Transacao { MetodoId = t.MetodoId, NifPagador = t.NifRecipiente, NifRecipiente = t.NifPagador, Valor = t.Valor, DataHora = DateTime.UtcNow });
                await DeleteReservaInParqueAPIAsync(reserva.ParqueId, reserva.ReservaParqueId);
                throw new Exception("O envio do email de confirmação falhou.");
            }

            try
            {
                return await _repository.PostAsync(reserva);
            }
            catch
            {
                await _pagamentoService.Reembolso(t);
                await _transacaoRepository.PostAsync(new Transacao { MetodoId = t.MetodoId, NifPagador = t.NifRecipiente, NifRecipiente = t.NifPagador, Valor = t.Valor, DataHora = DateTime.UtcNow });
                await DeleteReservaInParqueAPIAsync(reserva.ParqueId, reserva.ReservaParqueId);
                Utilizador utilizador = await _userManager.FindByIdAsync(reserva.NifUtilizador);
                _emailService.EnviarEmailCancelamento(utilizador.Nome, reserva.ReservaParqueId, utilizador.Email);
                throw new Exception("A reserva no Sistema Central falhou.");
            }
        }

        public async Task DeleteAsync(int id)
        {
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
                tReembolso = await _transacaoRepository.PostAsync(new Transacao { MetodoId = t.MetodoId, NifPagador = t.NifRecipiente, NifRecipiente = t.NifPagador, Valor = t.Valor, DataHora = DateTime.UtcNow });
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

        private async Task<QRCodeDTO> QRCodeDTOAsync(ReservaDTO reservaDTO, int reservaParqueId)
        {
            Utilizador utilizador = await _userManager.FindByIdAsync(reservaDTO.NifUtilizador);
            var l = GetLugarParqueByID(reservaDTO.LugarId, reservaDTO.ApiUrl).Result;
            var f = GetFreguesiaNomeByParqueID(l.ParqueId, reservaDTO.ApiUrl).Result;
            var p = GetParqueNomeByID(l.ParqueId, reservaDTO.ApiUrl).Result;
            QRCodeDTO qr = new QRCodeDTO { NomeUtilizador = utilizador.Nome, Email = utilizador.Email, IdReserva = reservaParqueId, Inicio = reservaDTO.Inicio, Fim = reservaDTO.Fim, NomeFreguesia = f, NomeParque = p, NumeroLugar = l.Numero, Fila = l.Fila, Andar = l.Andar };
            return qr;
        }
        private async Task<ReservaAPIParqueDTO> PostReservaInParqueAPIAsync(ReservaDTO reservaDTO)
        {
            ReservaAPIParqueDTO r = new ReservaAPIParqueDTO { NifCliente = 999999999, LugarId = reservaDTO.LugarId, Inicio = reservaDTO.Inicio, Fim = reservaDTO.Fim };
            ReservaAPIParqueDTO r2;
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(r), Encoding.UTF8, "application/json");
                string endpoint = reservaDTO.ApiUrl + "api/reservas";
                var response = await client.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();
                r2 = await response.Content.ReadAsAsync<ReservaAPIParqueDTO>();
            }
            return r2;
        }
        private async Task DeleteReservaInParqueAPIAsync(int parqueId, int reservaParqueId)
        {
            Parque parque = await _parqueRepository.GetByIdAsync(parqueId);
            using (HttpClient client = new HttpClient())
            {
                string endpoint1 = parque.ApiUrl + "api/reservas/" + reservaParqueId;
                var response1 = await client.DeleteAsync(endpoint1);
                response1.EnsureSuccessStatusCode();
            }
        }
        private async Task<string> GetFreguesiaNomeByParqueID(int id, string url)
        {
            FreguesiaDTO f;
            using (HttpClient client = new HttpClient())
            {
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
        private async Task<string> GetParqueNomeByID(int id, string url)
        {
            ParqueDTO p;
            using (HttpClient client = new HttpClient())
            {
                string endpoint1 = url + "api/parques/" + id;
                var response1 = await client.GetAsync(endpoint1);
                response1.EnsureSuccessStatusCode();
                p = await response1.Content.ReadAsAsync<ParqueDTO>();
            }
            return p.Rua;
        }
        private async Task<LugarDTO> GetLugarParqueByID(int id, string url)
        {
            LugarDTO l;
            using (HttpClient client = new HttpClient())
            {
                string endpoint1 = url + "api/lugares/" + id;
                var response1 = await client.GetAsync(endpoint1);
                response1.EnsureSuccessStatusCode();
                l = await response1.Content.ReadAsAsync<LugarDTO>();
            }
            return l;
        }
        private async Task<FreguesiaDTO> GetFreguesiaByNome(string nome, string url)
        {
            FreguesiaDTO f;
            using (HttpClient client = new HttpClient())
            {
                var listaFreguesias = new List<FreguesiaDTO>();
                string endpoint1 = url + "api/freguesias";
                var response1 = await client.GetAsync(endpoint1);
                response1.EnsureSuccessStatusCode();
                listaFreguesias = await response1.Content.ReadAsAsync<List<FreguesiaDTO>>();
                f = listaFreguesias.Find(z => z.Nome == nome);
            }
            return f;
        }
    }
}