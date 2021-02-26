using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APP_FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace APP_FrontEnd.Services
{
    public interface ITransacaoService
    {
        public Task<IEnumerable<Transacao>> GetAllTransacoesByNIF(string nif);
    }
}
