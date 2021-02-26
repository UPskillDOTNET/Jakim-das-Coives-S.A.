using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APP_FrontEnd.Services;
using Microsoft.AspNetCore.Identity;
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
            var lista = await _transacaoService.GetAllTransacoesByNIF();

            return View(lista);
        }
    }
}
