using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToTwitter;
using SentimentalTweets.Modelos;
using SentimentalTweets.Helpers;

namespace SentimentalTweets.Servicios
{
    public static class ServicioTwitter
    {
        private static async Task<IAuthorizer> AutorizarTwitter()
        {
            var auth = new ApplicationOnlyAuthorizer()
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = Constantes.TwitterApiKey,
                    ConsumerSecret = Constantes.TwitterApiSecret,
                },
            };

            await auth.AuthorizeAsync();

            return auth;
        }

        public static async Task<List<Tweet>> ObtenerTweets(string dato)
        {
            var twitter = new TwitterContext(await AutorizarTwitter());
            var tweets = new List<Tweet>();

            var busqueda = await (from t in twitter.Search
                                  where t.Type == SearchType.Search &&
                                  t.Query == dato
                                  select t).SingleOrDefaultAsync();

            if (busqueda != null && busqueda.Statuses != null)
            {
                busqueda.Statuses.ForEach(tweet =>
                            tweets.Add(new Tweet()
                            {
                                Mensaje = tweet.Text,
                                Idioma = tweet.Lang
                            }));
            }

            return tweets;
        }
    }
}

