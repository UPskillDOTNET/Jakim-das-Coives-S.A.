using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.Data;
using API_Sistema_Central.Models;

namespace API_Sistema_Central.Repositories
{
    public class MetodosPagamentoRepository : RepositoryBase<MetodoPagamento>, IMetodosPagamentoRepository
    {
        public MetodosPagamentoRepository(SCContext context) : base(context)
        {
        }
    }
}
