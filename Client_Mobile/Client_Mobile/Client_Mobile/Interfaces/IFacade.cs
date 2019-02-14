using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Interfaces
{
    using Enums;
    using Models;
    using ServiceModels;

    interface IFacade
    {
        ResponseBase Lock(LockerActionEnum action);
        ResponseBase Unlock(LockerActionEnum action);
        ResponseBase SendPinToLocker(Pin newPin);
        ResponseBase LoginUser(string password);
        ResponseBase CreateUser(string email, string pass);

        ResponseData<List<Parcel>> GetDeliveryHistory(string profileId);

        ResponseData<List<Pin>> GetActivePins(string profileId);



    }
}
