using System.Globalization;

namespace WarehouseApp.MAUI.Converters
{
    public class QuantityToProgressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int quantity &&
                parameter is string paramStr &&
                int.TryParse(paramStr, out int maxQuantity))
            {
                double ratio = Math.Clamp(quantity / (double)maxQuantity, 0, 1);
                return ratio;
            }
            return 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
