using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.Data;
using API_Sistema_Central.Models;

namespace API_Sistema_Central.Repositories
{
    public class PayPalRepository : RepositoryBase<PayPal>, IPayPalRepository
    {
        public PayPalRepository(SCContext context) : base(context)
        {
        }
    }
}
