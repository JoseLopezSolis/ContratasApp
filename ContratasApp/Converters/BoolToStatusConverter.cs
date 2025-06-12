using System.Globalization;

namespace ContratasApp.Converters;
public class BoolToStatusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is bool isClosed
            ? (isClosed ? "Esetado: Cerrado" : "Estado: Activo")
            : string.Empty;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}