using System.Threading.Tasks;
using API_Sistema_Central.DTOs;
using API_Sistema_Central.Models;
using API_Sistema_Central.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API_Sistema_Central.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadoresController : ControllerBase
    {
        private readonly UserManager<Utilizador> _userManager;
        private readonly SignInManager<Utilizador> _signInManager;
        private readonly ITokenService _tokenService;
        public UtilizadoresController(UserManager<Utilizador> userManager, SignInManager<Utilizador> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("Registar")]
        public async Task<ActionResult<TokenUtilizadorDTO>> RegistarUtilizador([FromBody] RegistarUtilizadorDTO model)
        {
            var infoUtilizadorDTO = new InfoUtilizadorDTO { Email = model.Email, Password = model.Password };
            var user = new Utilizador { Id = model.Nif, UserName = model.Email, Nome = model.Nome, Email = model.Email, CredencialId = model.CredencialId };
            var result = await _userManager.CreateAsync(user, model.Password);
            TokenUtilizadorDTO token = _tokenService.BuildToken(infoUtilizadorDTO);
            if (result.Succeeded)
            {
                return  token;
            }
            else
            {
                return BadRequest("Utilizador ou password inválidos");
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<TokenUtilizadorDTO>> Login([FromBody] InfoUtilizadorDTO infoUtilizadorDTO)
        {
            var result = await _signInManager.PasswordSignInAsync(infoUtilizadorDTO.Email, infoUtilizadorDTO.Password,
                 isPersistent: false, lockoutOnFailure: false);
            TokenUtilizadorDTO token = _tokenService.BuildToken(infoUtilizadorDTO);

            if (result.Succeeded)
            {
                return token;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "login inválido.");
                return BadRequest(ModelState);
            }
        }
    }
}