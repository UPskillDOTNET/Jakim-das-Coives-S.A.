using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.Data;
using API_Sistema_Central.Models;

namespace API_Sistema_Central.Repositories
{
    public interface IMetodoPagamentoRepository : IRepositoryBase<MetodoPagamento>
    {
    }
    public class MetodoPagamentoRepository : RepositoryBase<MetodoPagamento>, IMetodoPagamentoRepository
    {
        public MetodoPagamentoRepository(SCContext context) : base(context)
        {
        }
    }
}
