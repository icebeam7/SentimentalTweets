using System;
using System.Globalization;
using Xamarin.Forms;

namespace SentimentalTweets.Helpers
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double sentimiento = (double)value;

            if (sentimiento < 0.45)
                return Color.Red;
            else if (sentimiento < 0.55)
                return Color.Black;
            else
                return Color.Green;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}