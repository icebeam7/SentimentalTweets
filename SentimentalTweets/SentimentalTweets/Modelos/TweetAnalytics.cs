namespace SentimentalTweets.Modelos
{
    public class TweetAnalytics : Tweet
    {
        public double Sentimiento { get; set; }
        public string PalabrasClave { get; set; }
    }
}
