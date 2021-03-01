using System;
using System.Threading.Tasks;
using APP_FrontEnd.Models;
using APP_FrontEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APP_FrontEnd.Controllers
{
    public class UtilizadoresController : Controller
    {
        private readonly IUtilizadorService _utilizadorService;

        public UtilizadoresController(IUtilizadorService utilizadorService)
        {
            _utilizadorService = utilizadorService;
        }

        public async Task<IActionResult> Saldo()
        {
            var saldo = new Saldo();
            try
            {
                var valor = await _utilizadorService.GetSaldoAsync();
                saldo.Valor = valor;
                return View(saldo);
            }
            catch (Exception e)
            {
                return MensagemErro(e.Message);
            }
        }


        public IActionResult Depositar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Depositar([Bind("Valor")] Saldo saldo)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await _utilizadorService.DepositarSaldoAsync(saldo.Valor);
                    return RedirectToAction(nameof(Saldo));
                }
                catch (Exception e)
                {
                    return MensagemErro(e.Message);
                }                
            }
            return View(saldo);
        }

        private IActionResult MensagemErro(string mensagem)
        {
            return View("MensagemErro", new MensagemErro { Mensagem = mensagem });
        }
    }
}
