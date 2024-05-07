using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;

namespace UI.Converters
{
    public class UsagePercentageColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var usage = (double)value;
            switch (usage)
            {
                case var u when u >= 90:
                    return new SolidColorBrush(Colors.Red);
                case var u when u >= 75:
                    return new SolidColorBrush(Colors.Orange);
                default:
                    return new SolidColorBrush(Colors.Green);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
