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
        bool Lock(LockerActionEnum action);
        bool Unlock(LockerActionEnum action);
        bool SendPinToLocker(Pin newPin);
        ResponseBase LoginUser(string password);
        ResponseBase CreateUser(string email, string pass);

        ResponseData<List<Parcel>> GetDeliveryHistory(string profileId);

        ResponseData<List<Pin>> GetActivePins(string profileId);

        ResponseData<List<Locker>> GetLockers(string profileId);

        ResponseBase AddLockerToProfile(string profileId, Locker newLocker);



    }
}
