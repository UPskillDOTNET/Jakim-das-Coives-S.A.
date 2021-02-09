using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Paypal.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_Paypal.Services
{
    public interface IPaypalService
    {
        public Task<ActionResult<IEnumerable<Paypal>>> GetAllAsync();
        public Task<Paypal> GetByIdAsync(int id);
        public Task PutAsync(Paypal paypal);
        public Task<Paypal> PostAsync(Paypal paypal);
        public Task DeleteAsync(int id);
    }
}
