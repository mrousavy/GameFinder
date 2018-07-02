using System;
using System.Globalization;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;

namespace GameFinder.Converter
{
    [ValueConversion(typeof(long), typeof(PackIconKind))]
    public class SteamStateToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case long l:
                    return ToIcon((int) l);
                case int i:
                    return ToIcon(i);
                default:
                    throw new ArgumentException($"Invalid type! {value}");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case PackIcon _:
                    return 0;
                case PackIconKind _:
                    return 0;
                default:
                    throw new ArgumentException($"Invalid type! {value}");
            }
        }

        private static PackIconKind ToIcon(int i)
        {
            switch (i)
            {
                case 0:
                    return PackIconKind.ArrangeBringToFront;
                case 1:
                    return PackIconKind.Account;
                default:
                    throw new ArgumentOutOfRangeException($"Given State is not in range! {i}");
            }
        }
    }
}