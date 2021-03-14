using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Parque_Publico.Data;
using API_Parque_Publico.Models;

namespace API_Parque_Publico.Repositories
{
    public interface IParqueRepository : IRepositoryBase<Parque>
    {
    }
    public class ParqueRepository : RepositoryBase<Parque>, IParqueRepository
    {
        public ParqueRepository(API_Parque_PublicoContext context) : base(context)
        {
        }
    }
}
