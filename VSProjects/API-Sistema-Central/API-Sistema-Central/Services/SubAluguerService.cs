using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API_Sistema_Central.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API_Sistema_Central.Services
{
    public class SubAluguerService : ISubAluguerService
    {
        public async Task<ActionResult<IEnumerable<SubAluguerDTO>>> GetByNifAsync(string nif)
        {
            try
            {
                IEnumerable<SubAluguerDTO> result;
                using (HttpClient client = new HttpClient())
                {
                    string endpoint = "https://localhost:5005/api/lugares/all/" + nif;
                    var response = await client.GetAsync(endpoint);
                    response.EnsureSuccessStatusCode();
                    result = await response.Content.ReadAsAsync<IEnumerable<SubAluguerDTO>>();
                }
                return result.ToList();
            }
            catch (Exception)
            {
                throw new Exception("Não existem lugares associados a este NIF.");
            }
        }

        public async Task<ActionResult<SubAluguerDTO>> GetByIdAsync(int id)
        {
            try
            {
                SubAluguerDTO result;
                using (HttpClient client = new HttpClient())
                {
                    string endpoint = "https://localhost:5005/api/lugares/" + id;
                    var response = await client.GetAsync(endpoint);
                    response.EnsureSuccessStatusCode();
                    result = await response.Content.ReadAsAsync<SubAluguerDTO>();
                }
                return result;
            }
            catch (Exception)
            {
                throw new Exception("Este lugar não existe.");
            }
        }

        public async Task<SubAluguerDTO> PostSubAluguerAsync(SubAluguerDTO subAluguerDTO)
        {
            try
            {
                SubAluguerDTO lugar;
                using (HttpClient client = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(subAluguerDTO), Encoding.UTF8, "application/json");
                    string endpoint = "https://localhost:5005/api/lugares";
                    var response = await client.PostAsync(endpoint, content);
                    response.EnsureSuccessStatusCode();
                    lugar = await response.Content.ReadAsAsync<SubAluguerDTO>();
                }
                return lugar;
            }
            catch
            {
                throw new Exception("O registo do lugar para Sub-Aluguer falhou.");
            }
        }

        public async Task DeleteSubAluguerAsync(int id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string endpoint = "https://localhost:5005/api/lugares/" + id;
                    var response = await client.DeleteAsync(endpoint);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch
            {
                throw new Exception("O cancelamento do lugar para Sub-Aluguer falhou.");
            }
        }
    }
}
