using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Enums
{
    public enum LockerActionEnum
    {
        ManualOpen = 2,

        UserAppClose = 6,
        UserAppOpen = 7,
        UserAppOpened = 1,
        UserAppClosed = 0,
       
        Delivered = 3,
        PickedUp = 4,
        RequestPinApproval = 5,
      
        CheckConnection = 8,




    }
}
