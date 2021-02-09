using API_SubAluguer.Data;
using API_SubAluguer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Repository
{
    public class LugarRepository : RepositoryBase<Lugar>, ILugarRepository
    {
        public LugarRepository(API_SubAluguerContext context) : base(context)
        {
        }
    }
}
