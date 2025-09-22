using AppRpgEtec.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRpgEtec.Services.Usuarios
{
    public class UsuarioService : Request
    {
        private readonly Request _request;
        private const string _apiUrlBase = "https://rpgapi3ai2025.azurewebsites.net/Usuarios";
        //Azure: https://rpgapi3ai2025.azurewebsites.net/Usuarios
        //Somee: http://luizfernando.somee.com/RpgApi/Usuarios

        public UsuarioService()
        {
            _request = new Request();
        }

        private string _token = string.Empty;

        public UsuarioService(string token)
        {
            _request = new Request();
            _token = token;
        }

        public async Task<Usuario> PostRegistrarUsuarioAsync(Usuario u)
        {
            string urlComplementar = "/Registrar";
            u.Id = await _request.PostReturnIntAsync(_apiUrlBase + urlComplementar, u, string.Empty);

            return u;
        }

        public async Task<Usuario> PostAutenticarUsuarioAsync(Usuario u)
        {
            string urlComplementar = "/Autenticar";
            u = await _request.PostAsync(_apiUrlBase + urlComplementar, u, string.Empty);

            return u;
        }

        public async Task<int> PutAtualizarLocalizacaoAsync(Usuario u)
        {
            string urlComplementar = "/AtualizarLocalizacao";
            var result = await _request.PutAsync(_apiUrlBase + urlComplementar, u, _token);
            return result;
        }
        //using System.Collections.ObjectModel
        public async Task<ObservableCollection<Usuario>> GetUsuariosAsync()
        {
            string urlComplementar = string.Format("{0}", "/GetAll");
            ObservableCollection<Models.Usuario> listaUsuarios = await
            _request.GetAsync<ObservableCollection<Models.Usuario>>(_apiUrlBase + urlComplementar,
            _token);
            return listaUsuarios;
        }


    }
}
