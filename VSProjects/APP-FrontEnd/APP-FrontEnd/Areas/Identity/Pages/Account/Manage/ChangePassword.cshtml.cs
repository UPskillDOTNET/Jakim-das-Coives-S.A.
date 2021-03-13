using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using APP_FrontEnd.Models;
using APP_FrontEnd.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace APP_FrontEnd.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<Utilizador> _userManager;
        private readonly SignInManager<Utilizador> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;
        private readonly ITokenService _tokenService;

        public ChangePasswordModel(
            UserManager<Utilizador> userManager,
            SignInManager<Utilizador> signInManager,
            ILogger<ChangePasswordModel> logger,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _tokenService = tokenService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Palavra-passe actual")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "A palavra-passe deve ter no mínimo 6 caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Nova palavra-passe")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar nova palavra-passe")]
            [Compare("NewPassword", ErrorMessage = "A nova palavra-passe e a sua confirmação não correspondem.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (changePasswordResult.Succeeded)
            {
                try
                {
                    await AlterarPasswordAsync(new AlterarPasswordDTO { Nif = user.Id, PasswordActual = Input.OldPassword, PasswordNova = Input.NewPassword });
                }
                catch (Exception e)
                {
                    await _userManager.ChangePasswordAsync(user, Input.NewPassword, Input.OldPassword);
                    await _signInManager.RefreshSignInAsync(user);
                    StatusMessage = e.Message;

                    return RedirectToPage();
                }
            }
            else
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = "A tua palavra-passe foi alterada.";

            return RedirectToPage();
        }

        private async Task AlterarPasswordAsync(AlterarPasswordDTO alterarPasswordDTO)
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
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "aee2fb676a2e4b25a819af617eb64174");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(alterarPasswordDTO), Encoding.UTF8, "application/json");
                    string endpoint = "https://jakim-api-management.azure-api.net/sistema-central/api/utilizadores/alterar";
                    var response = await client.PostAsync(endpoint, content);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch
            {
                throw new Exception("Alteração de palavra-passe falhou no servidor. Volte a tentar.");
            }
        }
    }
}
