using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SentimentalTweets.Modelos;
using SentimentalTweets.Helpers;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace SentimentalTweets.Servicios
{
    public static class ServicioTextAnalytics
    {
        static HttpClient Cliente = new HttpClient();

        public static async Task<TweetAnalytics> AnalizarTweet(Tweet tweet)
        {
            try
            {
                var docs = PrepararDocumentos(tweet);

                var tweetAnalytics = new TweetAnalytics()
                {
                    Mensaje = tweet.Mensaje
                };

                var jsonSentimiento = await RealizarPeticionHttp(docs, "sentiment");
                var sentimiento = JObject.Parse(jsonSentimiento);
                tweetAnalytics.Sentimiento = double.Parse((string)sentimiento["documents"][0]["score"]);

                var jsonIdioma = await RealizarPeticionHttp(docs, "languages");
                var idioma = JObject.Parse(jsonIdioma);
                tweetAnalytics.Idioma = (string)idioma["documents"][0]["detectedLanguages"][0]["name"];

                var jsonPalabrasClave = await RealizarPeticionHttp(docs, "keyPhrases");
                var palabrasClave = JObject.Parse(jsonPalabrasClave);

                tweetAnalytics.PalabrasClave = (palabrasClave["documents"].HasValues)
                    ? string.Join(",", palabrasClave["documents"][0]["keyPhrases"])
                    : "N/A";

                return tweetAnalytics;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static async Task<string> RealizarPeticionHttp(string body, string servicio)
        {
            Cliente.BaseAddress = new Uri(Constantes.TextAnalyticsEndpoint);
            Cliente.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Constantes.TextAnalyticsApiKey);

            byte[] bytes = Encoding.UTF8.GetBytes(body);

            using (var content = new ByteArrayContent(bytes))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await Cliente.PostAsync(servicio, content);
                return await response.Content.ReadAsStringAsync();
            }
        }

        private static string PrepararDocumentos(Tweet tweet)
        {
            Document doc = new Document()
            {
                Id = "1",
                Language = tweet.Idioma,
                Text = tweet.Mensaje
            };

            var docs = new List<Document>() { doc };
            var wrapper = new { documents = docs };
            return JsonConvert.SerializeObject(wrapper);
        }
    }
}
