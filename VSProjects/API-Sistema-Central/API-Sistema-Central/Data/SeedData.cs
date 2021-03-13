using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.Models;
using Microsoft.AspNetCore.Identity;

namespace API_Sistema_Central.Data
{
    public class SeedData
    {
        public static void Initialize(SCContext context)
        {
            if (context.MetodosPagamento.Any())
            {
                return;
            }

            context.MetodosPagamento.Add(new MetodoPagamento { Nome = "Cartão de Crédito", ApiUrl = "https://jakim-api-management.azure-api.net/cartao/" });
            context.SaveChanges();
            context.MetodosPagamento.Add(new MetodoPagamento { Nome = "Débito Direto", ApiUrl = "https://jakim-api-management.azure-api.net/debito-direto/" });
            context.SaveChanges();
            context.MetodosPagamento.Add(new MetodoPagamento { Nome = "Paypal", ApiUrl = "https://jakim-api-management.azure-api.net/paypal/" });
            context.SaveChanges();
            context.MetodosPagamento.Add(new MetodoPagamento { Nome = "Carteira"});
            context.SaveChanges();

            context.Cartoes.Add(new Cartao { Numero = "0000000000000000", Nome = "Administrador", MetodoId = 1, Cvv = "000", DataValidade = "12/99" });
            context.SaveChanges();
            context.Cartoes.Add(new Cartao { Numero = "1111111111111111", Nome = "Teste Cartao", MetodoId = 1, Cvv = "111", DataValidade = "01/25" });
            context.SaveChanges();

            context.DebitosDiretos.Add(new DebitoDireto { Nome = "Teste Débito Direto", Rua = "Rua Teste", Freguesia = "Freguesia Teste", CodigoPostal = "4000-100", Iban = "PT50111111111111111111111", MetodoId = 2, DataSubscricao = DateTime.Now });
            context.SaveChanges();

            context.PayPal.Add(new PayPal { Email = "testepaypaljakim@gmail.com", Password = "123Pa$$word", MetodoId = 3 });
            context.SaveChanges();

            context.Parques.Add(new Parque { ApiUrl = "https://jakim-api-management.azure-api.net/parque-publico/" });
            context.SaveChanges();
            context.Parques.Add(new Parque { ApiUrl = "https://jakim-api-management.azure-api.net/parque-privado-1/" });
            context.SaveChanges();
            context.Parques.Add(new Parque { ApiUrl = "https://jakim-api-management.azure-api.net/parque-privado-2/" });
            context.SaveChanges();
            context.Parques.Add(new Parque { ApiUrl = "https://jakim-api-management.azure-api.net/parque-privado-3/" });
            context.SaveChanges();
            context.Parques.Add(new Parque { ApiUrl = "https://jakim-api-management.azure-api.net/sub-alugueres/" });
            context.SaveChanges();
        }

        public static async Task SeedRolesAsync(UserManager<Utilizador> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole("Administrador"));
        }

        public static async Task SeedUtilizadoresAsync(UserManager<Utilizador> userManager, RoleManager<IdentityRole> roleManager)
        {
            var administrador = new Utilizador { Id = "999999999", Nome = "Administrador", UserName = "sistemacentraljakim@gmail.com", Email = "sistemacentraljakim@gmail.com", CredencialId = 1 };
            if (userManager.Users.All(u => u.Id != administrador.Id))
            {
                var user = await userManager.FindByEmailAsync(administrador.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(administrador, "123Pa$$word");
                    await userManager.AddToRoleAsync(administrador, "Administrador");
                }
            }

            var teste1 = new Utilizador { Id = "111111111", Nome = "Teste Cartão de Crédito", UserName = "testecartaojakim@gmail.com", Email = "testecartaojakim@gmail.com", CredencialId = 2, Carteira = 1000 };
            if (userManager.Users.All(u => u.Id != teste1.Id))
            {
                var user = await userManager.FindByEmailAsync(teste1.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(teste1, "123Pa$$word");
                }
            }

            var teste2 = new Utilizador { Id = "111111112", Nome = "Teste Débito Direto", UserName = "testedebitodiretojakim@gmail.com", Email = "testedebitodiretojakim@gmail.com", CredencialId = 3, Carteira = 1000 };
            if (userManager.Users.All(u => u.Id != teste2.Id))
            {
                var user = await userManager.FindByEmailAsync(teste2.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(teste2, "123Pa$$word");
                }
            }

            var teste3 = new Utilizador { Id = "111111113", Nome = "Teste Paypal", UserName = "testepaypaljakim@gmail.com", Email = "testepaypaljakim@gmail.com", CredencialId = 4, Carteira = 1000 };
            if (userManager.Users.All(u => u.Id != teste3.Id))
            {
                var user = await userManager.FindByEmailAsync(teste3.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(teste3, "123Pa$$word");
                }
            }

            var removido = new Utilizador { Id = "000000000", Nome = "Utilizador removido", UserName = "utilizador@removido", Email = "utilizador@removido", CredencialId = 1};
            if (userManager.Users.All(u => u.Id != removido.Id))
            {
                var user = await userManager.FindByEmailAsync(removido.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(removido, "123Pa$$word");
                }
            }
        }
    }
}