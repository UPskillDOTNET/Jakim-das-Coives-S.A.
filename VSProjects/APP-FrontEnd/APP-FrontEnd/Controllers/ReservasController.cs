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

        public async Task<IActionResult> VerReservasDisponiveis(string freguesiaNome, DateTime inicio, DateTime fim)
        {
            try
            {
                var lista = await _reservaService.FindAvailableAsync(freguesiaNome, inicio, fim);
                return View(lista);
            }
            catch (Exception e)
            {
                return MensagemErro(e.Message);
            }
        }

        public IActionResult Reservar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reservar(ReservaDTO reservaDTO)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await _reservaService.PostAsync(reservaDTO);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    return MensagemErro(e.Message);
                }
            }
            return View(reservaDTO);
        }

        private IActionResult MensagemErro(string mensagem)
        {
            return View("MensagemErro", new MensagemErro { Mensagem = mensagem });
        }
    }
}
