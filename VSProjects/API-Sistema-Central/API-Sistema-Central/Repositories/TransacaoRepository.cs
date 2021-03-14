using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.Data;
using API_Sistema_Central.Models;

namespace API_Sistema_Central.Repositories
{
    public interface ITransacaoRepository : IRepositoryBase<Transacao>
    {
    }
    public class TransacaoRepository : RepositoryBase<Transacao>, ITransacaoRepository
    {
        public TransacaoRepository(SCContext context) : base(context)
        {
        }
    }
}
