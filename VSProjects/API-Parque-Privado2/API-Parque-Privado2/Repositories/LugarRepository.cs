using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Parque_Privado2.Data;
using API_Parque_Privado2.Models;

namespace API_Parque_Privado2.Repositories
{
    public interface ILugarRepository : IRepositoryBase<Lugar>
    {
    }
    public class LugarRepository : RepositoryBase<Lugar>, ILugarRepository
    {
        public LugarRepository(API_Parque_Privado2Context context) : base(context)
        {
        }
    }
}
