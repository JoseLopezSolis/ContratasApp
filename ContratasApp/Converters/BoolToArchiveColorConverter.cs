using System.Globalization;

namespace ContratasApp.Converters;

public class BoolToArchiveColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool archived && archived)
            return Colors.Gray;   // archivados
        if (Application.Current.Resources["CustomBlue"] is Color color)
            return color;    // activos
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}