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
        public Task<ActionResult<IEnumerable<Lugar>>> GetByNifAsync(string nif);
        public Task<Lugar> PostAsync(Lugar lugar);
        public Task DeleteAsync(int id);
        public IEnumerable<Lugar> FindAvailable(int freguesiaId, DateTime inicio, DateTime fim);
    }
}