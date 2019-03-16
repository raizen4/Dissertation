using System;
using System.Collections.Generic;
using System.Text;

namespace LockerApp.Enums
{
    public enum LockerActionRequestsEnum
    {   
        UserAppClose = 6,
        UserAppOpen = 7,
        RequestPin = 5,
        Delivered = 3,
        PickedUp = 4,
        UserAppOpened = 1,
        UserAppClosed = 0,
    }
}
