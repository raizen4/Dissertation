using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Converters
{
    using System.Globalization;
    using Enums;
    using Xamarin.Forms;

    public class ActionToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((LockerActionEnum)value == LockerActionEnum.Closed)
            {
                return "locked.png";
            }

            if ((LockerActionEnum)value == LockerActionEnum.Opened)
            {
                return "opened.png";
            }

            if ((LockerActionEnum)value == LockerActionEnum.Delivered)
            {
                return "shipped.png";
            }

            return "picked_up.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "Closed")
            {
                return LockerActionEnum.Closed;
            }
            if ((string)value == "Opened")
            {
                return LockerActionEnum.Opened;
            }
            if ((string)value == "Delivered")
            {
                return LockerActionEnum.Delivered;
            }
            return LockerActionEnum.PickedUp;
        }
    }

}
