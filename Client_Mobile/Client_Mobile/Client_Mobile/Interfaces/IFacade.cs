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
        Task<ResponseData<User>> LoginUser(string password, string email);
        Task<ResponseBase> CreateUser(string email, string pass, string profileId, string lockerId);

        Task<ResponseData<List<HistoryAction>>> GetDeliveryHistory();

        Task<ResponseData<List<Pin>>> GetActivePins();

      
        Task<Message> GetPendingMessagesFromHub();

        Task<ResponseBase> AddPinForLocker(Pin newPin);

        Task<ResponseBase> RemovePinForLocker(Pin newPin);

        Task<ResponseBase> AddNewActionForLocker(LockerActionEnum type);



    }
}
