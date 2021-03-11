using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using APP_FrontEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace APP_FrontEnd.Services
{
    public interface ITokenService
    {
        public Task<string> GetTokenAsync();
        public void SaveToken(string token);
        public Task<TokenResponse> GetTokenFromLoginAsync(InfoUtilizadorDTO info);
    }
    public class TokenService : ITokenService
    {
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IMemoryCache cache, IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetTokenAsync()
        {
            string token = string.Empty;

            if (!_cache.TryGetValue("TOKEN", out token))
            {
                string apitoken;
                try
                {
                    apitoken = await GetTokenFromRefreshTokenAsync();
                }
                catch
                {
                    throw new Exception("A sua sessão expirou. Volte a autenticar-se.");
                }

                SaveToken(apitoken);
                token = apitoken;
            }

            return token;
        }

        public void SaveToken(string token)
        {
            var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

            _cache.Set("TOKEN", token, options);
        }

        public async Task<TokenResponse> GetTokenFromLoginAsync(InfoUtilizadorDTO info)
        {
            try
            {
                TokenResponse tokenResponse;
                using (HttpClient client = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json");
                    string endpoint = "https://localhost:5050/api/utilizadores/login";
                    var response = await client.PostAsync(endpoint, content);
                    response.EnsureSuccessStatusCode();
                    tokenResponse = await response.Content.ReadAsAsync<TokenResponse>();
                    SaveCookie(tokenResponse.RefreshToken);
                }
                return tokenResponse;
            }
            catch
            {
                throw new Exception("Autenticação falhou no servidor. Volte a tentar.");
            }
        }

        private async Task<string> GetTokenFromRefreshTokenAsync()
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            var cookie = new Cookie { Name = "refreshToken", Value = refreshToken, Path = "/", Domain = "localhost", Expires = DateTime.UtcNow.AddMinutes(1), HttpOnly = true };
            var tokenResponse = new TokenResponse();
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(cookie);
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (HttpClient client = new HttpClient(handler))
            {
                string endpoint = "https://localhost:5050/api/utilizadores/refresh-token";
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                tokenResponse = await response.Content.ReadAsAsync<TokenResponse>();
            }
            SaveCookie(tokenResponse.RefreshToken);
            return tokenResponse.Token;
        }

        private void SaveCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
