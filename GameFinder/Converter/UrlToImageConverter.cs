using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameFinder.Converter
{
    [ValueConversion(typeof(string), typeof(ImageSource))]
    public class UrlToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Uri uri;
            switch (value)
            {
                case null:
                    return null;
                case string str:
                    uri = new Uri(str, UriKind.Absolute);
                    break;
                case Uri u:
                    uri = u;
                    break;
                default:
                    throw new ArgumentException($"Invalid type! {value}");

            }

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = uri;
            bitmap.EndInit();
            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case null:
                    return null;
                case BitmapImage img:
                    return img.UriSource;
                default:
                    throw new ArgumentException($"Invalid type! {value}");
            }
        }
    }
}
