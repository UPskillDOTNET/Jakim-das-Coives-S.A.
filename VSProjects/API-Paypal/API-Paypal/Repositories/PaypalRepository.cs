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
    public class PaypalRepository : IPaypalRepository
    {

        protected API_PaypalContext _context;
        public PaypalRepository(API_PaypalContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Paypal>>> GetAllAsync()
        {
            return await _context.Set<Paypal>().ToListAsync();
        }
        public async Task<Paypal> GetByIdAsync(int id)
        {
            var result = await _context.Set<Paypal>().FindAsync(id);
            return result;
        }
        public async Task PutAsync(Paypal entity)
        {
            _context.Set<Paypal>().Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<Paypal> PostAsync(Paypal entity)
        {
            EntityEntry<Paypal> entry;
            entry = await _context.Set<Paypal>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<Paypal>().FindAsync(id);
            _context.Set<Paypal>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
