using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Facade
{
    using Enums;
    using Interfaces;
    using Models;
    using ServiceModels;

    class ServicesFacade:IFacade
    {
        /// <inheritdoc />
        public bool Lock(LockerActionEnum action)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool Unlock(LockerActionEnum action)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool SendPinToLocker(Pin newPin)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ResponseBase LoginUser(string password)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ResponseBase CreateUser(string email, string pass)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ResponseData<List<Parcel>> GetDeliveryHistory(string profileId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ResponseData<List<Pin>> GetActivePins(string profileId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ResponseData<List<Locker>> GetLockers(string profileId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ResponseBase AddLockerToProfile(string profileId, Locker newLocker)
        {
            throw new NotImplementedException();
        }
    }
}
