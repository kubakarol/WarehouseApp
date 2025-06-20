using System.Globalization;
using Microsoft.Maui.Controls;

namespace WarehouseApp.MAUI.Converters
{
    public class QuantityToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int quantity)
            {
                if (quantity <= 3)
                    return Colors.Red;
                else if (quantity <= 5)
                    return Colors.Orange;
                else
                    return Colors.Green;
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
