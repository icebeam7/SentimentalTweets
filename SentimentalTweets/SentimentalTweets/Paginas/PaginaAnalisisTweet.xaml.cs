using SentimentalTweets.Modelos;
using SentimentalTweets.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SentimentalTweets.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaAnalisisTweet : ContentPage
    {
        Tweet tweet;

        public PaginaAnalisisTweet(Tweet tweet)
        {
            InitializeComponent();
            this.tweet = tweet;
        }

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            var tweetAnalytics = await ServicioTextAnalytics.AnalizarTweet(tweet);
            Loading(false);
            BindingContext = tweetAnalytics;
        }
    }
}