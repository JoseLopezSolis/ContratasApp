using System.Globalization;

namespace ContratasApp.Converters;

public class PaidButtonTextConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        (bool)value ? "Desmarcar como pagado" : "Marcar como pagado";

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}