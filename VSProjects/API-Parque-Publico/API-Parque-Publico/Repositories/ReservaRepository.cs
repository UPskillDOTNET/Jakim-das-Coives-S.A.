using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Parque_Publico.Data;
using API_Parque_Publico.Models;

namespace API_Parque_Publico.Repositories
{
    public interface IReservaRepository : IRepositoryBase<Reserva>
    {
    }
    public class ReservaRepository : RepositoryBase<Reserva>, IReservaRepository
    {
        public ReservaRepository(API_Parque_PublicoContext context) : base(context)
        {
        }
    }
}
