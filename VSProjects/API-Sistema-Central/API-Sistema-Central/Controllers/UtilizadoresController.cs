using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API_Sistema_Central.DTOs;
using API_Sistema_Central.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API_Sistema_Central.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadoresController : ControllerBase
    {
        private readonly UserManager<Utilizador> _userManager;
        private readonly SignInManager<Utilizador> _signInManager;
        private readonly IConfiguration _configuration;
        public UtilizadoresController(UserManager<Utilizador> userManager,
            SignInManager<Utilizador> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("Registar")]
        public async Task<ActionResult<TokenUtilizadorDTO>> RegistarUtilizador([FromBody] RegistarUtilizadorDTO model)
        {
            var info = new InfoUtilizadorDTO { Email = model.Email, Password = model.Password };
            var user = new Utilizador { Id = model.Nif, UserName = model.Nome, Nome = model.Nome, Email = model.Email, CredencialId = model.CredencialId };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return BuildToken(info);
            }
            else
            {
                return BadRequest("Utilizador ou password inválidos");
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<TokenUtilizadorDTO>> Login([FromBody] InfoUtilizadorDTO InfoUtilizadorDTO)
        {
            var result = await _signInManager.PasswordSignInAsync(InfoUtilizadorDTO.Email, InfoUtilizadorDTO.Password,
                 isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return BuildToken(InfoUtilizadorDTO);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "login inválido.");
                return BadRequest(ModelState);
            }
        }

        private TokenUtilizadorDTO BuildToken(InfoUtilizadorDTO InfoUtilizadorDTO)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, InfoUtilizadorDTO.Email),
                new Claim("meuValor", "oque voce quiser"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // tempo de expiração do token: 1 hora
            var expiration = DateTime.UtcNow.AddHours(1);
            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);
            return new TokenUtilizadorDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
