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
        // GET: SubAlugueresController
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

        // GET: SubAlugueresController/Details/5
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

        // GET: SubAlugueresController/Create
        public ActionResult Registar()
        {
            return View();
        }

        // POST: SubAlugueresController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registar([Bind("Id, ParqueId, Numero, Fila, Andar, Preco, NifProprietario, ReservaSistemaCentralId, Inicio, Fim")] SubAluguerDTO subAluguerDTO)
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


        // GET: SubAlugueresController/Delete/5
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

        // POST: SubAlugueresController/Delete/5
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
        private IActionResult MensagemErro(string mensagem)
        {
            return View("MensagemErro", new MensagemErro { Mensagem = mensagem });
        }
    }
}
