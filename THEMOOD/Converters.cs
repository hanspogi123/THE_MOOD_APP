using System.Globalization;
using Microsoft.Maui.Controls;

namespace THEMOOD.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isActive && parameter is string colors)
            {
                var colorParts = colors.Split(',');
                if (colorParts.Length == 2)
                {
                    var trueColor = GetColorFromName(colorParts[0].Trim());
                    var falseColor = GetColorFromName(colorParts[1].Trim());
                    return isActive ? trueColor : falseColor;
                }
            }
            return Colors.Pink;
        }

        private Color GetColorFromName(string colorName)
        {
            return colorName switch
            {
                "Black" => Colors.HotPink,
                "Gray" => Colors.White,
                "White" => Colors.White,
                // Add more colors as needed
                _ => Colors.Pink // Default
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToOpacityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                // Default values
                double trueValue = 1.0;
                double falseValue = 0.6;

                // Check if custom parameters are provided
                if (parameter is string paramString)
                {
                    var parts = paramString.Split(':');
                    if (parts.Length == 2)
                    {
                        double.TryParse(parts[0], out falseValue);
                        double.TryParse(parts[1], out trueValue);
                    }
                }

                return boolValue ? trueValue : falseValue;
            }

            return 0.6; // Default opacity
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return value;
        }
    }
}