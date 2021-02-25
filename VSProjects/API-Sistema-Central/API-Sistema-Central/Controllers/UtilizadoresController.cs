using System;
using System.Threading.Tasks;
using API_Sistema_Central.DTOs;
using API_Sistema_Central.Models;
using API_Sistema_Central.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API_Sistema_Central.Controllers
{
    [Route("api/utilizadores")]
    [ApiController]
    public class UtilizadoresController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUtilizadorService _utilizadorService;
        public UtilizadoresController(ITokenService tokenService, IUtilizadorService utilizadorService)
        {
            _tokenService = tokenService;
            _utilizadorService = utilizadorService;
        }

        [HttpPost("registar")]
        public async Task<ActionResult<TokenUtilizadorDTO>> RegistarUtilizador([FromBody] RegistarUtilizadorDTO registarUtilizadorDTO)
        {
            var infoUtilizadorDTO = new InfoUtilizadorDTO { Email = registarUtilizadorDTO.EmailUtilizador, Password = registarUtilizadorDTO.PasswordUtilizador };
            var result = await _utilizadorService.RegistarUtilizador(registarUtilizadorDTO);

            if (result.Succeeded)
            {
                return _tokenService.BuildToken(infoUtilizadorDTO);
            }
            else
            {
                return BadRequest("Utilizador ou password inválidos");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenUtilizadorDTO>> Login([FromBody] InfoUtilizadorDTO infoUtilizadorDTO)
        {
            var result = await _utilizadorService.Login(infoUtilizadorDTO);

            if (result.Succeeded)
            {
                return _tokenService.BuildToken(infoUtilizadorDTO);
            }
            else
            {
                return BadRequest("Login inválido");
            }
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

        [HttpPost("depositar/{nif}/{valor}")]
        public async Task<ActionResult> DepositarSaldoByNif(string nif, double valor)
        {
            try
            {
                await _utilizadorService.DepositarSaldoAsync(nif, valor);
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}