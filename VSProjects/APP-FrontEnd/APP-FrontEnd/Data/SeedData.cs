using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APP_FrontEnd.Models;
using Microsoft.AspNetCore.Identity;

namespace APP_FrontEnd.Data
{
    public class SeedData
    {
        public static async Task SeedUtilizadoresAsync(UserManager<Utilizador> userManager, RoleManager<IdentityRole> roleManager)
        {
            var administrador = new Utilizador { Id = "999999999", Nome = "Administrador", UserName = "sistemacentraljakim@gmail.com", Email = "sistemacentraljakim@gmail.com", MetodoId = 1 };
            if (userManager.Users.All(u => u.Id != administrador.Id))
            {
                var user = await userManager.FindByEmailAsync(administrador.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(administrador, "123Pa$$word");
                }
            }

            var teste1 = new Utilizador { Id = "111111111", Nome = "Teste Cartão de Crédito", UserName = "testecartaojakim@gmail.com", Email = "testecartaojakim@gmail.com", MetodoId = 1 };
            if (userManager.Users.All(u => u.Id != teste1.Id))
            {
                var user = await userManager.FindByEmailAsync(teste1.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(teste1, "123Pa$$word");
                }
            }

            var teste2 = new Utilizador { Id = "111111112", Nome = "Teste Débito Direto", UserName = "testedebitodiretojakim@gmail.com", Email = "testedebitodiretojakim@gmail.com", MetodoId = 2 };
            if (userManager.Users.All(u => u.Id != teste2.Id))
            {
                var user = await userManager.FindByEmailAsync(teste2.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(teste2, "123Pa$$word");
                }
            }

            var teste3 = new Utilizador { Id = "111111113", Nome = "Teste Paypal", UserName = "testepaypaljakim@gmail.com", Email = "testepaypaljakim@gmail.com", MetodoId = 3 };
            if (userManager.Users.All(u => u.Id != teste3.Id))
            {
                var user = await userManager.FindByEmailAsync(teste3.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(teste3, "123Pa$$word");
                }
            }
        }
    }
}
