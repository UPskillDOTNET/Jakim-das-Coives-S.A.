using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APP_FrontEnd.Models;

namespace APP_FrontEnd.Services
{
    public interface IUtilizadorService
    {
        public Task<SaldoDTO> GetSaldoAsync();
        public Task DepositarSaldoAsync(DepositarDTO depositar);
    }
}
