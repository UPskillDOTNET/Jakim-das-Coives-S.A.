using APP_FrontEnd.Models;
using APP_FrontEnd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP_FrontEnd.Controllers
{
    public class SubAlugueresController : Controller
    {
        private readonly ISubAluguerService _subAluguerService;
        public SubAlugueresController(ISubAluguerService subAluguerService)
        {
            _subAluguerService = subAluguerService;
        }
        
        public async Task<IActionResult> Index()
        {
            try
            {
                var lista = await _subAluguerService.GetAllSubAluguerByNIF();
                return View(lista);
            }
            catch (Exception e)
            {
                return MensagemErro(e.Message);
            }
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            try
            {
                var lugar = await _subAluguerService.GetSubAluguerById(id);
                return View(lugar);
            }
            catch (Exception e)
            {
                return MensagemErro(e.Message);
            }
        }

        public ActionResult Registar(DetalheReservaDTO d)
        {
            SubAluguerDTO subAluguerDTO = new SubAluguerDTO { NomeParque = d.NomeParque, Numero = d.NumeroLugar, Fila = d.Fila, Andar = d.Andar, NifProprietario = d.NifProprietario, ReservaSistemaCentralId = d.ReservaId, Inicio = d.Inicio, Fim = d.Fim };
            return View(subAluguerDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registar([Bind("Id, ParqueId, NomeParque, Numero, Fila, Andar, Preco, NifProprietario, ReservaSistemaCentralId, Inicio, Fim")] SubAluguerDTO subAluguerDTO)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    await _subAluguerService.PostSubAluguerAsync(subAluguerDTO);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    return MensagemErro(e.Message);
                }
            }
            return View(subAluguerDTO);
        }

        public async Task<IActionResult> Cancelar(int id)
        {
            try
            {
                var lugar = await _subAluguerService.GetSubAluguerById(id);
                return View(lugar);
            }
            catch (Exception e)
            {
                return MensagemErro(e.Message);
            }
        }

        [HttpPost, ActionName("Cancelar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelarConfirmado(int id)
        {
            try
            {
                await _subAluguerService.DeleteSubAluguerAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return MensagemErro(e.Message);
            }
        }


        public IActionResult VoltarAsReservas()
        {
            return RedirectToAction("Index", "Reservas");
        }

        private IActionResult MensagemErro(string mensagem)
        {
            return View("MensagemErro", new MensagemErro { Mensagem = mensagem });
        }
    }
}
