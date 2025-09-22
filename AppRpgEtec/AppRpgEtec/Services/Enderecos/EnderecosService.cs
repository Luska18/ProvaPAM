using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AppRpgEtec.Models;
using Newtonsoft.Json;

namespace AppRpgEtec.Services.Enderecos
{
    internal class EnderecosService
    {
        private readonly HttpClient _httpClient;

        public EnderecosService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<List<Endereco>> BuscarPorCep(string cep)
        {
            string url = $"https://nominatim.openstreetmap.org/search?format=json&q={cep}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var enderecos = JsonConvert.DeserializeObject<List<Endereco>>(content);

                    return enderecos;
                }
                else
                {
                    return new List<Endereco>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar endereço por CEP: {ex.Message}");
                return new List<Endereco>();
            }
        }
    }
}
