using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Converters
{
    using System.Globalization;
    using Enums;
    using Xamarin.Forms;

    public class IntToBoolConverterForCourier:IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((PinUserTypeEnum) value == PinUserTypeEnum.Courier)
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool) value)
            {
                return PinUserTypeEnum.Courier;
            }

            return PinUserTypeEnum.Friend;
        }
    }
}
