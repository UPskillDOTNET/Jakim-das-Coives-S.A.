using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP_FrontEnd.Services
{
    public interface IUtilizadorService
    {
        public Task<double> GetSaldoAsync();
        public Task DepositarSaldoAsync(double valor);
    }
}
