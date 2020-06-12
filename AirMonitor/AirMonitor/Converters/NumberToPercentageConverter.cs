using Xamarin.Forms;
using System;
using System.Globalization;

namespace AirMonitor
{
    public class NumberToPercentageConverter : BaseValueConverter<NumberToPercentageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int percentage = (int)value;
            return $"{percentage}%";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}