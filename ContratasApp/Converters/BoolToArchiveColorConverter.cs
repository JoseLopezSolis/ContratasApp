using System.Globalization;

namespace ContratasApp.Converters;

public class BoolToArchiveColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool archived && archived)
            return Colors.Gray;   // archivados
        return Colors.Green;      // activos
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}