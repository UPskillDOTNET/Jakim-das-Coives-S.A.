using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API_Sistema_Central.Services
{
    public interface ISubAluguerService
    {
        public Task<ActionResult<IEnumerable<SubAluguerDTO>>> GetByNifAsync(string nif);
        public Task<ActionResult<SubAluguerDTO>> GetByIdAsync(int id);
        public Task<SubAluguerDTO> PostSubAluguerAsync(SubAluguerDTO subAluguerDTO);
        public Task DeleteSubAluguerAsync(int id);
    }
}
