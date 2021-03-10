using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Linq;
using System.Threading.Tasks;
using APP_FrontEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using APP_FrontEnd.Services;

namespace APP_FrontEnd.Areas.Identity.Pages.Account.Manage
{
    public partial class MetodoPagamentoModel : PageModel
    {
        private readonly UserManager<Utilizador> _userManager;
        private readonly SignInManager<Utilizador> _signInManager;
        private readonly ITokenService _tokenService;

        public MetodoPagamentoModel(UserManager<Utilizador> userManager, SignInManager<Utilizador> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Insira a sua palavra-passe para validar.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            public int MetodoId { get; set; }

            // Dados Cartão de Crédito
            [RegularExpression(@"^\d{16}$", ErrorMessage = "Numero do cartão de crédito inválido")]
            public string Numero { get; set; }
            public string NomeCartao { get; set; }

            [RegularExpression(@"^(0[1-9]|1[0-2])[/]\d{2}$", ErrorMessage = "Data de Validade do cartão de crédito inválido")]
            public string DataValidade { get; set; }

            [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV do cartão de crédito inválido")]
            public string Cvv { get; set; }

            // Dados Debito Direto
            [RegularExpression(@"^[P][T][5][0]\d{21}$", ErrorMessage = "IBAN inválido")]
            public string Iban { get; set; }
            public string NomeDebitoDireto { get; set; }
            public string Rua { get; set; }

            [RegularExpression(@"^\d{4}-\d{3}$", ErrorMessage = "Código postal inválido")]
            public string CodigoPostal { get; set; }
            public string Freguesia { get; set; }
            [DataType(DataType.DateTime)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]

            public DateTime DataSubscricao { get; set; }

            // Dados PayPal
            [DataType(DataType.EmailAddress)]
            public string EmailPayPal { get; set; }

            [DataType(DataType.Password)]
            public string PasswordPayPal { get; set; }
        }

        private void LoadAsync(Utilizador user)
        {
            Input = new InputModel
            {
                MetodoId = user.MetodoId
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostChangeMetodoPagamentoAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                LoadAsync(user);
                return Page();
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            string password;
            bool validado;
            if (hasPassword)
            {
                password = Input.Password;
                validado = (await _signInManager.PasswordSignInAsync(user.Email, password, isPersistent: false, lockoutOnFailure: false)).Succeeded;
            }
            else
            {
                password = user.Id + user.MetodoId + user.Nome + "$PP$";
                validado = true;
            }
            if (validado)
            {
                int metodoIdAntigo = user.MetodoId;
                try
                {
                    user.MetodoId = Input.MetodoId;
                    await _userManager.UpdateAsync(user);
                }
                catch
                {
                    StatusMessage = "Alteração do método de pagamento preferencial falhou. Volte a tentar.";
                    return RedirectToPage();
                }

                var dto = new AlterarMetodoPagamentoDTO
                {
                    Email = user.Email,
                    Password = password,
                    MetodoId = Input.MetodoId,
                    Numero = Input.Numero,
                    NomeCartao = Input.NomeCartao,
                    DataValidade = Input.DataValidade,
                    Cvv = Input.Cvv,
                    Iban = Input.Iban,
                    NomeDebitoDireto = Input.NomeDebitoDireto,
                    Rua = Input.Rua,
                    CodigoPostal = Input.CodigoPostal,
                    DataSubscricao = Input.DataSubscricao,
                    Freguesia = Input.Freguesia,
                    EmailPayPal = Input.EmailPayPal,
                    PasswordPayPal = Input.PasswordPayPal
                };

                try
                {
                    await AlterarMetodoPagamentoAsync(dto);
                }
                catch (Exception e)
                {
                    user.MetodoId = metodoIdAntigo;
                    await _userManager.UpdateAsync(user);
                    StatusMessage = e.Message;
                    return RedirectToPage();
                }

                StatusMessage = "O seu método de pagamento preferencial foi alterado com sucesso.";
                return RedirectToPage();
            }

            StatusMessage = "O seu método de pagamento preferencial não foi alterado.";
            return RedirectToPage();
        }

        private async Task AlterarMetodoPagamentoAsync(AlterarMetodoPagamentoDTO dto)
        {
            string token;
            try
            {
                token = await _tokenService.GetTokenAsync();
            }
            catch (Exception e)
            {
                await _signInManager.SignOutAsync();
                throw new Exception(e.Message);
            }
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                    string endpoint = "https://localhost:5050/api/utilizadores/metodo";
                    var response = await client.PostAsync(endpoint, content);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch
            {
                throw new Exception("Alteração do método de pagamento preferencial falhou no servidor. Volte a tentar.");
            }
        }
    }
}
