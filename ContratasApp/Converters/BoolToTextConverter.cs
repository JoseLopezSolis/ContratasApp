using System.Globalization;

namespace ContratasApp.Converters;

public class BoolToTextConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isPaid)
            return isPaid ? "Desmarcar como pagado" : "Marcar como pagado";
        return "Estado desconocido";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}