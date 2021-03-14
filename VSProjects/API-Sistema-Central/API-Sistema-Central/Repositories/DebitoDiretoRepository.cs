using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.Data;
using API_Sistema_Central.Models;

namespace API_Sistema_Central.Repositories
{
    public interface IDebitoDiretoRepository : IRepositoryBase<DebitoDireto>
    {
    }
    public class DebitoDiretoRepository : RepositoryBase<DebitoDireto>, IDebitoDiretoRepository
    {
        public DebitoDiretoRepository(SCContext context) : base(context)
        {
        }
    }
}
