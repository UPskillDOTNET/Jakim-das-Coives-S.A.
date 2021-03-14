using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Parque_Privado.Data;
using API_Parque_Privado.Models;

namespace API_Parque_Privado.Repositories
{
    public interface IParqueRepository : IRepositoryBase<Parque>
    {
    }
    public class ParqueRepository : RepositoryBase<Parque>, IParqueRepository
    {
        public ParqueRepository(API_Parque_PrivadoContext context) : base(context)
        {
        }
    }
}
