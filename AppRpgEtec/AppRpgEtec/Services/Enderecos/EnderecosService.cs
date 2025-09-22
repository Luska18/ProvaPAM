using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            string url = $"https://nominatim.openstreetmap.org/search?format=json&q=";
            var response = await _httpClient.GetAsync(url);
            return JsonConvert.DeserializeObject<List<Endereco>>(response);
        }
    }
}
