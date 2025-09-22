using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRpgEtec.Models;
using AppRpgEtec.Services.Enderecos;
using AppRpgEtec.Services.Usuarios;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace AppRpgEtec.ViewModels.Usuarios
{
    class LocalizacaoViewModel : BaseViewModel
    {
        private UsuarioService uService;
        private EnderecosService enderecoService;

        private string cep;
        public string Cep
        {
            get => cep;
            set
            {
                if (cep != value)
                {
                    cep = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand BuscarEnderecoCommand { get; }

        public LocalizacaoViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            uService = new UsuarioService(token);
            enderecoService = new EnderecosService();

            BuscarEnderecoCommand = new Command(async () => await BuscarEndereco());
        }

        private Map meuMapa;

        public Map MeuMapa
        {
            get => meuMapa;
            set
            {
                if (value != null)
                {
                    meuMapa = value;
                    OnPropertyChanged();
                }
            }
        }

        public async void InicializarMapa()
        {
            try
            {
                Location location = new Location(-23.5200241d, -46.596498d);
                Pin pinEtec = new Pin()
                {
                    Type = PinType.Place,
                    Label = "Etec Horácio",
                    Address = "Rua alcantara, 113, vila Guilherme",
                    Location = location
                };

                Map map = new Map();
                MapSpan mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(5));
                map.Pins.Add(pinEtec);
                map.MoveToRegion(mapSpan);

                MeuMapa = map;
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", e.Message, "OK");
            }
        }

        public async void ExibirUsuarioNoMapa()
        {
            try
            {
                ObservableCollection<Usuario> ocUsuarios = await uService.GetUsuariosAsync();
                List<Usuario> listaUsuarios = new List<Usuario>(ocUsuarios);
                Map map = new Map();

                foreach (Usuario u in listaUsuarios)
                {
                    if (u.lat != null && u.lon != null)
                    {
                        double lat = (double)u.lat;
                        double lon = (double)u.lon;
                        Location location = new Location(lat, lon);

                        Pin pinAtual = new Pin()
                        {
                            Type = PinType.Place,
                            Label = u.Username,
                            Address = $"E-mail: {u.Email}",
                            Location = location
                        };
                        map.Pins.Add(pinAtual);
                    }
                }
                MeuMapa = map;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }
        public async Task BuscarEndereco()
        {
            if (string.IsNullOrEmpty(Cep))
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Informe um CEP válido.", "OK");
                return;
            }

            try
            {
                var endereco = await enderecoService.BuscarPorCep(Cep);

                if (endereco != null)
                {
                    Location location = new Location(endereco.lat, endereco.lon);
                    Pin pinEndereco = new Pin()
                    {
                        Type = PinType.Place,
                        Label = "Localização Pesquisada",
                        Address = endereco.EnderecoCompleto,
                        Location = location
                    };

                    Map map = new Map();
                    map.Pins.Add(pinEndereco);
                    MapSpan mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(5));
                    map.MoveToRegion(mapSpan);

                    MeuMapa = map;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", "Endereço não encontrado para o CEP informado.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }
}
