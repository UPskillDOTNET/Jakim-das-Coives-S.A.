using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Paypal.Data;
using API_Paypal.Models;

namespace API_Paypal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaypalController : ControllerBase
    {
        private readonly API_PaypalContext _context;

        public PaypalController(API_PaypalContext context)
        {
            _context = context;
        }

        // GET: api/Paypal
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paypal>>> GetPaypal()
        {
            return await _context.Paypal.ToListAsync();
        }

        // POST: api/Paypal
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Paypal>> PostPaypal(Paypal paypal)
        {
            _context.Paypal.Add(paypal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaypal", new { id = paypal.Id }, paypal);
        }




        /*GET: api/Paypal/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Paypal>> GetPaypal(int id)
        {
            var paypal = await _context.Paypal.FindAsync(id);

            if (paypal == null)
            {
                return NotFound();
            }

            return paypal;
        }*/
    }
}
