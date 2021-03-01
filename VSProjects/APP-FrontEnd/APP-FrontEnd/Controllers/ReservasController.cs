using System;
using System.Threading.Tasks;
using APP_FrontEnd.Models;
using APP_FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace APP_FrontEnd.Controllers
{
    public class ReservasController : Controller
    {
        private readonly IReservaService _reservaService;

        public ReservasController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var lista = await _reservaService.GetByNifAsync();
                return View(lista);
            }
            catch (Exception e)
            {
                return MensagemErro(e.Message);
            }
        }

        private IActionResult MensagemErro(string mensagem)
        {
            return View("MensagemErro", new MensagemErro { Mensagem = mensagem });
        }
    }
}
