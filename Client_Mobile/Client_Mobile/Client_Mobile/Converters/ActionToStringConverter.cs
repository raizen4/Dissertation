using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Converters
{
    using System.Globalization;
    using Enums;
    using Xamarin.Forms;

    public class ActionToStringConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((LockerActionEnum)value== LockerActionEnum.UserAppClosed)
            {
                return "Closed";
            }

            if((LockerActionEnum)value == LockerActionEnum.UserAppOpened)
            {
                return "Opened";
            }

            if ((LockerActionEnum) value == LockerActionEnum.Delivered)
            {
                return "Delivered";
            }

            return "Picked Up";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
