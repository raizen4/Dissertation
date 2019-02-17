using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Facade
{
    using System.Threading.Tasks;
    using Enums;
    using Interfaces;
    using Models;
    using ServiceModels;

    class ServicesFacade:IFacade
    {

        private readonly IApiWrapper apiWrapper;

        private readonly IIoTHub iotHub;
        public ServicesFacade(IApiWrapper apiWrapper, IIoTHub iotHub)
        {
            this.apiWrapper = apiWrapper;
            this.iotHub = iotHub;


        }

        /// <inheritdoc />
       

        /// <inheritdoc />
        public async Task<bool> Lock(LockerActionEnum action)
        {
           
            var result = await this.iotHub.Lock("121321", action);
            if (result)
            {
                return true;
            }

            return false;


        }

        /// <inheritdoc />
        public async Task<bool> Unlock(LockerActionEnum action)
        {
          
                var result = await this.iotHub.Unlock("121321", action);
                if (result)
                {
                    return true;
                }
            

            return false;
        }

        /// <inheritdoc />
        public async Task<bool> SendPinToLocker(Pin newPin)
        {
                var result = await this.iotHub.SendPinToLocker("121321", LockerActionEnum.NewPinGenerated,
                   newPin);
                if (result)
                {
                    return true;
                }

            return false;
        }

        /// <inheritdoc />
        public Task<ResponseBase> LoginUser(string password)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public  Task<ResponseBase> CreateUser(string email, string pass)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<ResponseData<List<Parcel>>> GetDeliveryHistory(string profileId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<ResponseData<List<Pin>>> GetActivePins(string profileId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<ResponseData<List<Locker>>> GetLockers(string profileId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public  Task<ResponseBase> AddLockerToProfile(string profileId, Locker newLocker)
        {
            throw new NotImplementedException();
        }
    }
}
