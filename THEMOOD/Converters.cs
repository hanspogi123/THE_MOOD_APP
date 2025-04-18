using System.Globalization;
using Microsoft.Maui.Controls; // Make sure this using directive is present

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
                "Black" => Colors.DeepPink,
                "Gray" => Colors.Black,
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
            if (value is bool isActive)
            {
                return isActive ? 2.0 : 0.6; // Adjust opacity values as needed
            }
            return 0.6; // Default opacity
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}