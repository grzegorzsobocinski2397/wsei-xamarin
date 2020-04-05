using Xamarin.Forms;
using System;
using System.Globalization;

namespace AirMonitor
{
    public class NumberToPercentageConverter : BaseValueConverter<NumberToPercentageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double percentage = (double)value * 100;
            return $"{percentage}%";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}