using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using APP_FrontEnd.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using APP_FrontEnd.Services;
using Microsoft.AspNetCore.Http;

namespace APP_FrontEnd.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<Utilizador> _userManager;
        private readonly SignInManager<Utilizador> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly ITokenService _tokenService;

        public LoginModel(SignInManager<Utilizador> signInManager, ILogger<LoginModel> logger, UserManager<Utilizador> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _tokenService = tokenService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var info = new InfoUtilizadorDTO { Email = Input.Email, Password = Input.Password };
                    var token = GetTokenAsync(info).Result;
                    _tokenService.SaveToken(token.Token);

                    _logger.LogInformation("O utilizador iniciou sessão.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("A sua conta de utilizador foi bloqueada.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Inicio de sessão inválido.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task<TokenResponse> GetTokenAsync(InfoUtilizadorDTO info)
        {
            try
            {
                TokenResponse token;
                using (HttpClient client = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json");
                    string endpoint = "https://localhost:5050/api/utilizadores/login";
                    var response = await client.PostAsync(endpoint, content);
                    response.EnsureSuccessStatusCode();
                    token = await response.Content.ReadAsAsync<TokenResponse>();
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddDays(7)
                    };
                    Response.Cookies.Append("refreshToken", token.RefreshToken, cookieOptions);
                }
                return token;
            }
            catch
            {
                throw new Exception("Autenticação falhou no servidor. Volte a tentar.");
            }
        }
    }
}
