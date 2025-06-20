using System.Globalization;

namespace WarehouseApp.MAUI.Converters
{
    public class QuantityToProgressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int quantity)
                return Math.Min(quantity / 100.0, 1.0); // max 100
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
