using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace AirMonitor
{
    /// <summary>
    /// Base value converter for other converters.
    /// </summary>
    public abstract class BaseValueConverter<T> : IMarkupExtension, IValueConverter
        where T : class, new()
    {
        #region Value Converter 

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
        #endregion
        #region MarkupExtension
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new T());
        }
        private static T converter = null;
        #endregion

    }
}