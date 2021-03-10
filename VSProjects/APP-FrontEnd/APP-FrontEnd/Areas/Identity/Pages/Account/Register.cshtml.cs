using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
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
using APP_FrontEnd.Services;
using Microsoft.AspNetCore.Http;

namespace APP_FrontEnd.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Utilizador> _signInManager;
        private readonly UserManager<Utilizador> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ITokenService _tokenService;

        public RegisterModel(UserManager<Utilizador> userManager, SignInManager<Utilizador> signInManager, ILogger<RegisterModel> logger, IEmailSender emailSender, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _tokenService = tokenService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF inválido")]
            [Display(Name = "NIF")]
            public string Nif { get; set; }

            [Required]
            [Display(Name = "Nome Completo")]
            public string NomeUtilizador { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar Password")]
            [Compare("Password", ErrorMessage = "As passwords não coincidem. Por favor insire novamente.")]
            public string ConfirmPassword { get; set; }

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

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new Utilizador { Id = Input.Nif, UserName = Input.Email, Email = Input.Email, MetodoId = Input.MetodoId, Nome = Input.NomeUtilizador };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    TokenResponse token;
                    try
                    {
                        token = await RegistarUtilizadorNoSCAsync(new RegistarUtilizadorDTO
                        {
                            Nif = Input.Nif,
                            NomeUtilizador = Input.NomeUtilizador,
                            EmailUtilizador = Input.Email,
                            PasswordUtilizador = Input.Password,
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
                    _tokenService.SaveToken(token.Token);
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddDays(7)
                    };
                    Response.Cookies.Append("refreshToken", token.RefreshToken, cookieOptions);

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task<TokenResponse> RegistarUtilizadorNoSCAsync(RegistarUtilizadorDTO registarUtilizadorDTO)
        {
            TokenResponse token;
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(registarUtilizadorDTO), Encoding.UTF8, "application/json");
                string endpoint = "https://localhost:5050/api/utilizadores/registar";
                var response = await client.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();
                token = await response.Content.ReadAsAsync<TokenResponse>();
            }
            return token;
        }
    }
}
