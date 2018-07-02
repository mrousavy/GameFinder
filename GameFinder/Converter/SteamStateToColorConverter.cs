using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GameFinder.Converter
{
    [ValueConversion(typeof(int), typeof(Brush))]
    public class SteamStateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case long l:
                    return ToColor((int) l);
                case int i:
                    return ToColor(i);
                default:
                    throw new ArgumentException($"Invalid type! {value}");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case Color _:
                    return 0;
                case Brush _:
                    return 0;
                default:
                    throw new ArgumentException($"Invalid type! {value}");
            }
        }

        private static Brush ToColor(int i)
        {
            switch (i)
            {
                case 0:
                    return new SolidColorBrush(Colors.Gray);
                case 1:
                    return new SolidColorBrush(Colors.Green);
                case 2:
                    return new SolidColorBrush(Colors.DodgerBlue);
                case 3:
                    return new SolidColorBrush(Colors.Red);
                case 4:
                    return new SolidColorBrush(Colors.Gray);
                default:
                    throw new ArgumentOutOfRangeException($"Given State is not in range! {i}");
            }
        }
    }
}