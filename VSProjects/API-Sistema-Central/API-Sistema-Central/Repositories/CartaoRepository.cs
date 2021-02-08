using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.Data;
using API_Sistema_Central.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Sistema_Central.Repositories
{
    public class CartaoRepository : RepositoryBase<Cartao>, ICartaoRepository
    {
        public CartaoRepository(SCContext context):base(context)
        {
        }
    }
}
