using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using APP_FrontEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace APP_FrontEnd.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<Utilizador> _signInManager;
        private readonly UserManager<Utilizador> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExternalLoginModel(
            SignInManager<Utilizador> signInManager,
            UserManager<Utilizador> userManager,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender,
            IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF inválido")]
            [Display(Name = "NIF")]
            public string Nif { get; set; }

            [Required]
            [Display(Name = "Nome Completo")]
            public string NomeUtilizador { get; set; }
            [Required]
            [Range(typeof(int), "1", "3")]
            [Display(Name = "Método de Pagamento Preferencial")]
            public int MetodoId { get; set; }

            // Dados Cartão de Crédito
            [RegularExpression(@"^\d{16}$", ErrorMessage = "Número do cartão de crédito inválido")]
            [Display(Name = "Número do Cartão")]
            public string Numero { get; set; }

            [Display(Name = "Nome do Titular")]
            public string NomeCartao { get; set; }

            [RegularExpression(@"^(0[1-9]|1[0-2])[/]\d{2}$", ErrorMessage = "Data de Validade do cartão de crédito inválido")]
            [Display(Name = "Data de Validade")]
            public string DataValidade { get; set; }

            [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV do cartão de crédito inválido")]
            [Display(Name = "CVV")]
            public string Cvv { get; set; }

            // Dados Debito Direto
            [RegularExpression(@"^[P][T][5][0]\d{21}$", ErrorMessage = "IBAN inválido")]
            [Display(Name = "IBAN")]
            public string Iban { get; set; }

            [Display(Name = "Nome do Titular")]
            public string NomeDebitoDireto { get; set; }
            public string Rua { get; set; }
            [RegularExpression(@"^\d{4}-\d{3}$", ErrorMessage = "Código postal inválido")]
            [Display(Name = "Código Postal")]
            public string CodigoPostal { get; set; }
            public string Freguesia { get; set; }

            // Dados PayPal
            [DataType(DataType.EmailAddress)]
            [Display(Name = "Email")]
            public string EmailPayPal { get; set; }
            [DataType(DataType.Password)]
            [Display(Name = "Password do Paypal")]
            public string PasswordPayPal { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new {ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor : true);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                var infoDTO = new InfoUtilizadorDTO { Email = user.Email, Password = user.Id + user.MetodoId + user.Nome + "$PP$" };
                var token = GetTokenAsync(infoDTO).Result;
                user.Token = token.Token;
                user.Expiration = token.Expiration;
                await _userManager.UpdateAsync(user);


                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = new Utilizador { Id = Input.Nif, UserName = Input.Email, Email = Input.Email, MetodoId = Input.MetodoId, Nome = Input.NomeUtilizador };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        TokenUtilizadorDTO token;
                        try
                        {
                            token = await RegistarUtilizadorNoSCAsync(new RegistarUtilizadorDTO
                            {
                                Nif = Input.Nif,
                                NomeUtilizador = Input.NomeUtilizador,
                                EmailUtilizador = Input.Email,
                                PasswordUtilizador = Input.Nif + Input.MetodoId + Input.NomeUtilizador + "$PP$",
                                MetodoId = Input.MetodoId,
                                Numero = Input.Numero,
                                NomeCartao = Input.NomeCartao,
                                DataValidade = Input.DataValidade,
                                Cvv = Input.Cvv,
                                Iban = Input.Iban,
                                NomeDebitoDireto = Input.NomeDebitoDireto,
                                Rua = Input.Rua,
                                CodigoPostal = Input.CodigoPostal,
                                Freguesia = Input.Freguesia,
                                DataSubscricao = DateTime.UtcNow,
                                EmailPayPal = Input.EmailPayPal,
                                PasswordPayPal = Input.PasswordPayPal
                            });
                        }
                        catch (Exception)
                        {
                            await _userManager.DeleteAsync(user);
                            throw new Exception("O registo no servidor falhou.");
                        }
                        var registeredUser = await _userManager.FindByIdAsync(Input.Nif);
                        registeredUser.Token = token.Token;
                        registeredUser.Expiration = token.Expiration;
                        await _userManager.UpdateAsync(registeredUser);


                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);

                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
        }
        private async Task<TokenUtilizadorDTO> RegistarUtilizadorNoSCAsync(RegistarUtilizadorDTO registarUtilizadorDTO)
        {
            TokenUtilizadorDTO token;
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(registarUtilizadorDTO), Encoding.UTF8, "application/json");
                string endpoint = "https://localhost:5050/api/utilizadores/registar";
                var response = await client.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();
                token = await response.Content.ReadAsAsync<TokenUtilizadorDTO>();
            }
            return token;
        }
        private async Task<TokenUtilizadorDTO> GetTokenAsync(InfoUtilizadorDTO info)
        {
            try
            {
                TokenUtilizadorDTO token;
                using (HttpClient client = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json");
                    string endpoint = "https://localhost:5050/api/utilizadores/login";
                    var response = await client.PostAsync(endpoint, content);
                    response.EnsureSuccessStatusCode();
                    token = await response.Content.ReadAsAsync<TokenUtilizadorDTO>();
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
