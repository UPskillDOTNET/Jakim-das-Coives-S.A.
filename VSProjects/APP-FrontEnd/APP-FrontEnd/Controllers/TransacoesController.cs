using System;
using System.Threading.Tasks;
using APP_FrontEnd.Models;
using APP_FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace APP_FrontEnd.Controllers
{
    public class TransacoesController : Controller
    {
        private readonly ITransacaoService _transacaoService;

        public TransacoesController(ITransacaoService transacaoService)
        {
            _transacaoService = transacaoService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var lista = await _transacaoService.GetAllTransacoesByNIF();
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
