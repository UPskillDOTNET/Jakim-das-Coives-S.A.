using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Paypal.Data;
using API_Paypal.Models;
using API_Paypal.Services;

namespace API_Paypal.Controllers
{
    [Route("api/paypal")]
    [ApiController]
    public class PaypalController : ControllerBase
    {
        private readonly IPaypalService _service;

        public PaypalController(IPaypalService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paypal>>> GetAllPaypal()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Paypal>> GetPaypal(int id)
        {
            var paypal = await _service.GetByIdAsync(id);

            if (paypal == null)
            {
                return NotFound();
            }

            return paypal;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaypal(int id, Paypal paypal)
        {
            if (id != paypal.Id)
            {
                return BadRequest();
            }
            try
            {
                await _service.PutAsync(paypal);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Paypal>> PostPaypal(Paypal paypal)
        {
            await _service.PostAsync(paypal);

            return CreatedAtAction("GetPaypal", new { id = paypal.Id }, paypal);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaypal(int id)
        {
            var paypal = await _service.GetByIdAsync(id);
            if (paypal == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
