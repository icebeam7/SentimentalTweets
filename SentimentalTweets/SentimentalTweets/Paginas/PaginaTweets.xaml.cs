using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SentimentalTweets.Servicios;
using SentimentalTweets.Modelos;

namespace SentimentalTweets.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaTweets : ContentPage
    {
        public PaginaTweets()
        {
            InitializeComponent();
        }

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        public async void btnBuscar_Clicked(object sender, EventArgs e)
        {
            var busqueda = txtBusqueda.Text;

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                Loading(true);
                var tweets = await ServicioTwitter.ObtenerTweets(busqueda);
                lsvTweets.ItemsSource = tweets;
                Loading(false);
            }
            else
            {
                await DisplayAlert("Advertencia", "Debes introducir un término de búsqueda", "OK");
            }
        }

        private async void lsvTweets_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var tweet = (Tweet)e.SelectedItem;
                await Navigation.PushAsync(new PaginaAnalisisTweet(tweet));
            }
            catch (Exception ex)
            {
            }
        }
    }
}