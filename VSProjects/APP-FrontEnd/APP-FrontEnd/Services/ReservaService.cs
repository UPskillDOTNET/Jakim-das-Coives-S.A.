using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using APP_FrontEnd.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace APP_FrontEnd.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<Utilizador> _userManager;
        private readonly SignInManager<Utilizador> _signInManager;

        public ReservaService(IHttpContextAccessor httpContextAccessor, UserManager<Utilizador> userManager, SignInManager<Utilizador> signInManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ActionResult<IEnumerable<LugarDTO>>> FindAvailableAsync(string freguesiaNome, DateTime inicio, DateTime fim)
        {
            string nif;
            try
            {
                nif = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            catch
            {
                throw new Exception("Utilizador não tem login feito.");
            }

            var token = await GetTokenByNif(nif);

            var listaLugares = new List<LugarDTO>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string endpoint = "https://localhost:5050/api/reservas/disponibilidade/" + freguesiaNome + "/" + inicio.ToString("yyyy-MM-ddTHH:mm:ss") + "/" + fim.ToString("yyyy-MM-ddTHH:mm:ss");
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                listaLugares = await response.Content.ReadAsAsync<List<LugarDTO>>();
            }
            return listaLugares;

        }

        public async Task<ActionResult<IEnumerable<DetalheReservaDTO>>> GetByNifAsync()
        {
            string nif;
            try
            {
                nif = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            catch
            {
                throw new Exception("Utilizador não tem login feito.");
            }

            var token = await GetTokenByNif(nif);

            var listaReservas = new List<DetalheReservaDTO>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string endpoint = "https://localhost:5050/api/reservas/all/" + nif;
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                listaReservas = await response.Content.ReadAsAsync<List<DetalheReservaDTO>>();
            }
            return listaReservas;
        }

        public async Task<DetalheReservaDTO> GetByIdAsync(int id)
        {
            string nif;
            try
            {
                nif = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            catch
            {
                throw new Exception("Utilizador não tem login feito.");
            }

            var token = await GetTokenByNif(nif);

            var result = new DetalheReservaDTO();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string endpoint = "https://localhost:5050/api/reservas/" + id;
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                result = await response.Content.ReadAsAsync<DetalheReservaDTO>();
            }
            return result;
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
            Transacao t = await _transacaoRepository.PostAsync(new Transacao { NifPagador = reservaDTO.NifComprador, NifRecipiente = reservaDTO.NifVendedor, Valor = reserva.Custo, MetodoId = reservaDTO.MetodoId, DataHora = DateTime.UtcNow });
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
                await _transacaoRepository.PostAsync(new Transacao { MetodoId = t.MetodoId, NifPagador = t.NifRecipiente, NifRecipiente = t.NifPagador, Valor = t.Valor, DataHora = DateTime.UtcNow });
                throw new Exception("A reserva no parque de destino falhou.");
            }

            //Enviar email de confirmacao
            try
            {
                QRCodeDTO qr = QRCodeDTOAsync(reservaDTO, reserva.ReservaParqueId, l, p.ApiUrl).Result;
                if (p.ApiUrl == "https://localhost:5005/")
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
                await _transacaoRepository.PostAsync(new Transacao { MetodoId = t.MetodoId, NifPagador = t.NifRecipiente, NifRecipiente = t.NifPagador, Valor = t.Valor, DataHora = DateTime.UtcNow });
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
                await _transacaoRepository.PostAsync(new Transacao { MetodoId = t.MetodoId, NifPagador = t.NifRecipiente, NifRecipiente = t.NifPagador, Valor = t.Valor, DataHora = DateTime.UtcNow });
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
        private async Task<string> GetTokenByNif(string nif)
        {
            var user = await _userManager.FindByIdAsync(nif);
            if (user.Expiration < DateTime.UtcNow)
            {
                await _signInManager.SignOutAsync();
                throw new Exception("A sua sessão expirou. Volte a autenticar-se.");
            }
            else
            {
                return user.Token;
            }
        }
    }
}