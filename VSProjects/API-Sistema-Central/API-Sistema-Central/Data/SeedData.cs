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

            context.MetodosPagamento.Add(new MetodoPagamento { Nome = "Cartão de Crédito", ApiUrl = "https://localhost:5021/" });
            context.SaveChanges();
            context.MetodosPagamento.Add(new MetodoPagamento { Nome = "Débito Direto", ApiUrl = "https://localhost:5022/" });
            context.SaveChanges();
            context.MetodosPagamento.Add(new MetodoPagamento { Nome = "Paypal", ApiUrl = "https://localhost:5020/" });
            context.SaveChanges();
            context.MetodosPagamento.Add(new MetodoPagamento { Nome = "Carteira"});
            context.SaveChanges();

            context.Cartoes.Add(new Cartao { Numero = "0000000000000000", Nome = "Administrador", MetodoId = 1, Cvv = "000", DataValidade = "12/99" });
            context.SaveChanges();
            context.Cartoes.Add(new Cartao { Numero = "1111111111111111", Nome = "Teste Cartao", MetodoId = 1, Cvv = "111", DataValidade = "01/25" });
            context.SaveChanges();

            context.DebitosDiretos.Add(new DebitoDireto { Nome = "Teste Débito Direto", Rua = "Rua Teste", Freguesia = "Freguesia Teste", CodigoPostal = "4000-100", Iban = "PT50111111111111111111111", MetodoId = 2, DataSubscricao = DateTime.Now });
            context.SaveChanges();

            context.PayPal.Add(new PayPal { Email = "testepaypal@upskill.pt", Password = "123Pa$$word", MetodoId = 3 });
            context.SaveChanges();

            context.Parques.Add(new Parque { ApiUrl = "https://localhost:5001/" });
            context.SaveChanges();
            context.Parques.Add(new Parque { ApiUrl = "https://localhost:5002/" });
            context.SaveChanges();
            context.Parques.Add(new Parque { ApiUrl = "https://localhost:5003/" });
            context.SaveChanges();
            context.Parques.Add(new Parque { ApiUrl = "https://localhost:5004/" });
            context.SaveChanges();
            context.Parques.Add(new Parque { ApiUrl = "https://localhost:5005/" });
            context.SaveChanges();
        }

        public static async Task SeedRolesAsync(UserManager<Utilizador> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole("Administrador"));
        }

        public static async Task SeedUtilizadoresAsync(UserManager<Utilizador> userManager, RoleManager<IdentityRole> roleManager)
        {
            var administrador = new Utilizador { Id = "999999999", Nome = "Administrador", UserName = "administrador@upskill.pt", Email = "administrador@upskill.pt", CredencialId = 1 };
            if (userManager.Users.All(u => u.Id != administrador.Id))
            {
                var user = await userManager.FindByEmailAsync(administrador.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(administrador, "123Pa$$word");
                    await userManager.AddToRoleAsync(administrador, "Administrador");
                }
            }

            var teste1 = new Utilizador { Id = "111111111", Nome = "Teste Cartão de Crédito", UserName = "testecartao@upskill.pt", Email = "testecartao@upskill.pt", CredencialId = 2, Carteira = 1000 };
            if (userManager.Users.All(u => u.Id != teste1.Id))
            {
                var user = await userManager.FindByEmailAsync(teste1.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(teste1, "123Pa$$word");
                }
            }

            var teste2 = new Utilizador { Id = "111111112", Nome = "Teste Débito Direto", UserName = "testedebitodireto@upskill.pt", Email = "testedebitodireto@upskill.pt", CredencialId = 3, Carteira = 1000 };
            if (userManager.Users.All(u => u.Id != teste2.Id))
            {
                var user = await userManager.FindByEmailAsync(teste2.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(teste2, "123Pa$$word");
                }
            }

            var teste3 = new Utilizador { Id = "111111113", Nome = "Teste Paypal", UserName = "testepaypal@upskill.pt", Email = "testepaypal@upskill.pt", CredencialId = 4, Carteira = 1000 };
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