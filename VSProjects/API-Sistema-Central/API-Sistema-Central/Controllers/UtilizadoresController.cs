using System;
using System.Threading.Tasks;
using API_Sistema_Central.Authentication;
using API_Sistema_Central.DTOs;
using API_Sistema_Central.Models;
using API_Sistema_Central.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API_Sistema_Central.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/utilizadores")]
    [ApiController]
    public class UtilizadoresController : ControllerBase
    {
        private readonly IUtilizadorService _utilizadorService;
        public UtilizadoresController(IUtilizadorService utilizadorService)
        {
            _utilizadorService = utilizadorService;
        }

        [AllowAnonymous]
        [HttpPost("registar")]
        public async Task<IActionResult> RegistarUtilizador([FromBody] RegistarUtilizadorDTO registarUtilizadorDTO)
        {
            var response = await _utilizadorService.RegistarUtilizador(registarUtilizadorDTO, IpAddress());

            if (response == null)
            {
                return BadRequest("Utilizador ou password inválidos");
            }

            SetTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] InfoUtilizadorDTO infoUtilizadorDTO)
        {
            var response = await _utilizadorService.Login(infoUtilizadorDTO, IpAddress());

            if (response == null)
            {
                return BadRequest("Login inválido");
            }

            SetTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _utilizadorService.RefreshTokenAsync(refreshToken, IpAddress());

            if (response == null)
            {
                return Unauthorized("Token inválido");
            }

            SetTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [HttpPost("rescindir-token")]
        public async Task<IActionResult> RevokeTokenAsync([FromBody] RevokeTokenRequest model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token é necessário");
            }

            var response = await _utilizadorService.RevokeTokenAsync(token, IpAddress());

            if (!response)
            {
                return NotFound("Token não encontrado");
            }
            
            return Ok("Token rescindido");
        }


        [HttpGet("saldo/{nif}")]
        public async Task<ActionResult<double>> GetSaldoByNif(string nif)
        {
            try
            {
                return await _utilizadorService.GetSaldoAsync(nif);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }

        }

        [HttpPost("depositar")]
        public async Task<ActionResult> DepositarSaldoByNif(DepositarDTO depositar)
        {
            try
            {
                await _utilizadorService.DepositarSaldoAsync(depositar.Nif, depositar.Valor);
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("reset")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                await _utilizadorService.ResetPasswordAsync(resetPasswordDTO);
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("alterar")]
        public async Task<ActionResult> AlterarPassword(AlterarPasswordDTO alterarPasswordDTO)
        {
            try
            {
                await _utilizadorService.AlterarPasswordAsync(alterarPasswordDTO);
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("nome")]
        public async Task<ActionResult> AlterarNome(AlterarNomeDTO alterarNomeDTO)
        {
            try
            {
                await _utilizadorService.AlterarNomeAsync(alterarNomeDTO);
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("metodo")]
        public async Task<ActionResult> AlterarMetodoPagamento(AlterarMetodoPagamentoDTO alterarMetodoPagamentoDTO)
        {
            try
            {
                await _utilizadorService.AlterarMetodoPagamentoAsync(alterarMetodoPagamentoDTO);
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }


        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}