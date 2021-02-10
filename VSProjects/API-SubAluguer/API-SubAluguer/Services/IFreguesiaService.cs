using API_SubAluguer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Services
{
    public interface IFreguesiaService
    {
        public Task<ActionResult<IEnumerable<Freguesia>>> GetAllAsync();
        public Task<Freguesia> GetByIdAsync(int id);
        public Task PutAsync(Freguesia freguesia);
        public Task<Freguesia> PostAsync(Freguesia freguesia);
        public Task DeleteAsync(int id);
    }
}