using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Paypal.Models;
using API_Paypal.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API_Paypal.Services
{
    public class PaypalService : IPaypalService
    {
        private readonly IPaypalRepository _repository;

        public PaypalService(IPaypalRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActionResult<IEnumerable<Paypal>>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Paypal> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task PutAsync(Paypal paypal)
        {
            await _repository.PutAsync(paypal);
        }

        public async Task<Paypal> PostAsync(Paypal paypal)
        {
            return await _repository.PostAsync(paypal);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
