using System.Globalization;

namespace InventoryApp;

public class StatusColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var status = value as string;
        return status?.ToLower() switch
        {
            "open" => Colors.Orange,
            "closed" or "done" => Colors.Green,
            _ => Colors.Gray
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
