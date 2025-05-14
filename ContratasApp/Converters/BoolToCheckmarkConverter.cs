using System.Globalization;

namespace ContratasApp.Converters;

/// <summary>
/// Converts a boolean to a checkmark ("✓") if true, or an empty string if false.
/// </summary>
public class BoolToCheckmarkConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool b && b)
            return "✓";
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Not needed for one-way binding
        throw new NotSupportedException();
    }
}