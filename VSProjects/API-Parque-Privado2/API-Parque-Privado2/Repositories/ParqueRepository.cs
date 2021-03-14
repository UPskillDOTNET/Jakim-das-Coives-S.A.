using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Parque_Privado2.Data;
using API_Parque_Privado2.Models;

namespace API_Parque_Privado2.Repositories
{
    public interface IParqueRepository : IRepositoryBase<Parque>
    {
    }
    public class ParqueRepository : RepositoryBase<Parque>, IParqueRepository
    {
        public ParqueRepository(API_Parque_Privado2Context context) : base(context)
        {
        }
    }
}
