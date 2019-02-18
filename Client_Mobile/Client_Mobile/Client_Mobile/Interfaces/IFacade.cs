using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Interfaces
{
    using System.Threading.Tasks;
    using Enums;
    using Microsoft.Azure.Devices.Client;
    using Models;
    using ServiceModels;

    public interface IFacade
    {

        Task<bool> Lock();
        Task<bool> Unlock();
        Task<bool> SendPinToLocker(Pin newPin);
        Task<ResponseBase> LoginUser(string password);
        Task<ResponseBase> CreateUser(string email, string pass);

        Task<ResponseData<List<Parcel>>> GetDeliveryHistory(string profileId);

        Task<ResponseData<List<Pin>>> GetActivePins(string profileId);

        Task<ResponseData<List<Locker>>> GetLockers(string profileId);

        Task<ResponseBase> AddLockerToProfile(string profileId, Locker newLocker);
        Task<Message> GetPendingMessagesFromHub();



    }
}
