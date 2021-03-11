using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.Data;
using API_Sistema_Central.Models;

namespace API_Sistema_Central.Repositories
{
    public interface ICartaoRepository : IRepositoryBase<Cartao>
    {
    }
    public class CartaoRepository : RepositoryBase<Cartao>, ICartaoRepository
    {
        public CartaoRepository(SCContext context):base(context)
        {
        }
    }
}
