using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using APP_FrontEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace APP_FrontEnd.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<Utilizador> _userManager;

        public ResetPasswordModel(UserManager<Utilizador> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "A palavra-passe deve ter no mínimo 6 caracteres.", MinimumLength = 6)]
            [Display(Name = "Palavra-passe")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar palavra-passe")]
            [Compare("Password", ErrorMessage = "A nova palavra-passe e a sua confirmação não correspondem.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                return BadRequest("Tem que fornecer um código para redefinir a sua password.");
            }
            else
            {
                Input = new InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                var token = GetTokenAsync(new InfoUtilizadorDTO { Email = "sistemacentraljakim@gmail.com", Password = "123Pa$$word" }).Result;
                await ResetPasswordAsync(new ResetPasswordDTO { Nif = user.Id, Password = Input.Password }, token.Token);

                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }

        private async Task<TokenResponse> GetTokenAsync(InfoUtilizadorDTO info)
        {
            try
            {
                TokenResponse token;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "aee2fb676a2e4b25a819af617eb64174");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json");
                    string endpoint = "https://jakim-api-management.azure-api.net/sistema-central/api/utilizadores/login";
                    var response = await client.PostAsync(endpoint, content);
                    response.EnsureSuccessStatusCode();
                    token = await response.Content.ReadAsAsync<TokenResponse>();
                }
                return token;
            }
            catch
            {
                throw new Exception("Autenticação falhou no servidor. Volte a tentar.");
            }
        }
        private async Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO, string token)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "aee2fb676a2e4b25a819af617eb64174");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(resetPasswordDTO), Encoding.UTF8, "application/json");
                    string endpoint = "https://jakim-api-management.azure-api.net/sistema-central/api/utilizadores/reset";
                    var response = await client.PostAsync(endpoint, content);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch
            {
                throw new Exception("Repor palavra-passe falhou no servidor. Volte a tentar.");
            }
        }
    }
}
