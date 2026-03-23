using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;

namespace StarWarsApi.Converters;

/// <summary>
/// Converts a URL string into a <see cref="BitmapImage"/> for binding to <c>Image.Source</c>.
///
/// Returns null when the string is empty or null so the Image control renders nothing
/// and the fallback emoji layer beneath remains visible.
///
/// Sets <see cref="BitmapImage.DecodePixelWidth"/> to 320 px to reduce memory usage
/// when rendering cards at typical display sizes.
/// </summary>
public sealed class StringToImageSourceConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not string { Length: > 0 } url)
            return null;

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            return null;

        return new BitmapImage(uri)
        {
            DecodePixelWidth  = 320,
            DecodePixelType   = DecodePixelType.Logical
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => throw new NotSupportedException();
}
