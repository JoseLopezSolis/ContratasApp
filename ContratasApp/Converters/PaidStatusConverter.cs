using System.Globalization;

namespace ContratasApp.Converters;

public class PaidStatusConverter: IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)=>
        (bool)value ? "✔️ Estado: Pagado" : "🔁 Estado: Pendiente";

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => 
        throw new NotImplementedException();
}