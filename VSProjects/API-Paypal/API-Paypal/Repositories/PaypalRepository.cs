using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Paypal.Models;
using API_Paypal.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
    public class PaypalRepository : IPaypalRepository
    {

        protected API_PaypalContext _context;
        public PaypalRepository(API_PaypalContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Paypal>>> GetAllAsync()
        {
            return await _context.Paypal.ToListAsync();
        }
        public async Task<Paypal> GetByIdAsync(int id)
        {
            var result = await _context.Paypal.FindAsync(id);
            return result;
        }
        public async Task PutAsync(Paypal entity)
        {
            _context.Paypal.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<Paypal> PostAsync(Paypal entity)
        {
            EntityEntry<Paypal> entry;
            entry = await _context.Paypal.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Paypal.FindAsync(id);
            _context.Paypal.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
