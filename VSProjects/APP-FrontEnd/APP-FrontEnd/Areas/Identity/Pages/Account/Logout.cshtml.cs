using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using APP_FrontEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Http;
using APP_FrontEnd.Services;
using System.Net.Http.Headers;

namespace APP_FrontEnd.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<Utilizador> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;

        public LogoutModel(SignInManager<Utilizador> signInManager, ILogger<LogoutModel> logger, IHttpContextAccessor httpContextAccessor, ITokenService tokenService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await RevokeTokenAsync();

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }

        private async Task RevokeTokenAsync()
        {
            string token = "";
            try
            {
                token = await _tokenService.GetTokenAsync();
            }
            catch
            {
            }

            if (token != "")
            {
                try
                {
                    var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
                    var cookie = new Cookie { Name = "refreshToken", Value = refreshToken, Path = "/", Domain = "localhost", Expires = DateTime.UtcNow.AddMinutes(1), HttpOnly = true };
                    var tokenResponse = new TokenResponse();
                    var cookieContainer = new CookieContainer();
                    cookieContainer.Add(cookie);
                    using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
                    using (HttpClient client = new HttpClient(handler))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        StringContent content = new StringContent(JsonConvert.SerializeObject(new RevokeTokenRequest { Token = refreshToken }), Encoding.UTF8, "application/json");
                        string endpoint = "https://localhost:5050/api/utilizadores/rescindir-token";
                        var response = await client.PostAsync(endpoint, content);
                        response.EnsureSuccessStatusCode();
                    }
                    Response.Cookies.Delete("refreshToken");
                }
                catch
                {
                    throw new Exception("Rescindir refresh token falhou no servidor. Volte a tentar.");
                }
            }
        }
    }
}
