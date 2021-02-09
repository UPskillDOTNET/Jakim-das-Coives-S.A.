using API_SubAluguer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Services
{
    public interface ILugarService
    {
        public Task<ActionResult<IEnumerable<Lugar>>> GetAllAsync();
        public Task<Lugar> GetByIdAsync(int id);
        public Task PutAsync(Lugar lugar);
        public Task<Lugar> PostAsync(Lugar lugar);
        public Task DeleteAsync(int id);
    }
}