using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace StarWarsApi.Converters;

/// <summary>
/// Converts a hex color string (e.g. "#4FC3F7") into a <see cref="SolidColorBrush"/>.
/// Used to drive per-category accent colors on result cards from ViewModel data.
/// </summary>
public sealed class HexStringToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string hex && hex.Length == 7 && hex[0] == '#')
        {
            try
            {
                var r = System.Convert.ToByte(hex[1..3], 16);
                var g = System.Convert.ToByte(hex[3..5], 16);
                var b = System.Convert.ToByte(hex[5..7], 16);
                return new SolidColorBrush(Color.FromArgb(255, r, g, b));
            }
            catch { /* fall through to default */ }
        }

        return new SolidColorBrush(Colors.Gray);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => throw new NotSupportedException();
}
