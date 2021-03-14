using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Parque_Privado.Data;
using API_Parque_Privado.Models;

namespace API_Parque_Privado.Repositories
{
    public interface IReservaRepository : IRepositoryBase<Reserva>
    {
    }
    public class ReservaRepository : RepositoryBase<Reserva>, IReservaRepository
    {
        public ReservaRepository(API_Parque_PrivadoContext context) : base(context)
        {
        }
    }
}
