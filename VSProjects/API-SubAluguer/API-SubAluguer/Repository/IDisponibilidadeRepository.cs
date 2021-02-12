using API_SubAluguer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Repository
{
    public interface IDisponibilidadeRepository : IRepositoryBase<Disponibilidade>
    {
        public Task<ActionResult<IEnumerable<Disponibilidade>>> GetAllIncludeAsync();
    }
}
