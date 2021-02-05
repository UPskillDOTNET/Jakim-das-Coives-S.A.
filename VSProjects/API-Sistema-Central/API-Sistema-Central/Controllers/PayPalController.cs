using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Sistema_Central.Data;
using API_Sistema_Central.Models;

namespace API_Sistema_Central.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayPalController : ControllerBase
    {
        private readonly SCContext _context;

        public PayPalController(SCContext context)
        {
            _context = context;
        }

        // GET: api/PayPal
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PayPal>>> GetPayPal()
        {
            return await _context.PayPal.ToListAsync();
        }

        // GET: api/PayPal/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PayPal>> GetPayPal(int id)
        {
            var payPal = await _context.PayPal.FindAsync(id);

            if (payPal == null)
            {
                return NotFound();
            }

            return payPal;
        }

        // PUT: api/PayPal/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayPal(int id, PayPal payPal)
        {
            if (id != payPal.Id)
            {
                return BadRequest();
            }

            _context.Entry(payPal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PayPalExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PayPal
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PayPal>> PostPayPal(PayPal payPal)
        {
            _context.PayPal.Add(payPal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayPal", new { id = payPal.Id }, payPal);
        }

        // DELETE: api/PayPal/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayPal(int id)
        {
            var payPal = await _context.PayPal.FindAsync(id);
            if (payPal == null)
            {
                return NotFound();
            }

            _context.PayPal.Remove(payPal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PayPalExists(int id)
        {
            return _context.PayPal.Any(e => e.Id == id);
        }
    }
}
