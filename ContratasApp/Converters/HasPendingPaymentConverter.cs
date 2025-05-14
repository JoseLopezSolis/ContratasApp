using System.Collections;
using System.Globalization;
using ContratasApp.Models;

namespace ContratasApp.Converters;

public class HasPendingPaymentsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IEnumerable list &&
            list.Cast<PaymentSchedule>().Any(p => !p.IsPaid))
        {
            return true;
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}