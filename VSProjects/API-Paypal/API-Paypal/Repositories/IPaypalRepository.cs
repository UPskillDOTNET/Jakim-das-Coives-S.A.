using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Paypal.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_Paypal.Repositories
{
    public interface IPaypalRepository
    {
        public Task<ActionResult<IEnumerable<Paypal>>> GetAllAsync();
        public Task<Paypal> GetByIdAsync(int id);
        public Task PutAsync(Paypal entity);
        public Task<Paypal> PostAsync(Paypal entity);
        public Task DeleteAsync(int id);
    }
}
