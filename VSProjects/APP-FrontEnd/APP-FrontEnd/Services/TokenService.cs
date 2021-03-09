using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using APP_FrontEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace APP_FrontEnd.Services
{
    public interface ITokenService
    {
        public Task<string> GetTokenAsync();
        public void SaveToken(string token);
    }
    public class TokenService : ITokenService
    {
        private readonly IMemoryCache _cache;

        public TokenService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<string> GetTokenAsync()
        {
            string token = string.Empty;

            if (!_cache.TryGetValue("TOKEN", out token))
            {
                string apitoken;
                try
                {
                    apitoken = await GetTokenFromApiAsync();
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

        private async Task<string> GetTokenFromApiAsync()
        {
            var tokenResponse = new TokenResponse();
            using (HttpClient client = new HttpClient())
            {
                string endpoint = "https://localhost:5050/api/utilizadores/refresh-token";
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                tokenResponse = await response.Content.ReadAsAsync<TokenResponse>();
            }
            return tokenResponse.Token;
        }
    }
}
