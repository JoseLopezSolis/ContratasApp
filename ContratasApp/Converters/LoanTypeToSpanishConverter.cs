using System.Globalization;
using ContratasApp.Enums;

namespace ContratasApp.Converters;

public class LoanTypeToSpanishConverter: IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            LoanType.Weekly => "Semanal",
            LoanType.Monthly => "Mensual",
            _ => value?.ToString()
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}