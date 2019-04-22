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
        Task<ResponseData<User>> LoginUser(string password, string email);
        Task<ResponseBase> CreateUser(string email, string pass, string displayName, string phone);

        Task<ResponseData<List<HistoryAction>>> GetDeliveryHistory();

        Task<ResponseData<List<Pin>>> GetActivePins();

      
        Task<LockerMessage> GetPendingMessagesFromHub();

        Task<ResponseBase> AddPinForLocker(Pin newPin);

        Task<ResponseBase> RemovePinForLocker(Pin pinToRemove);

        Task<ResponseBase> AddNewActionForLocker(LockerActionEnum type);



    }
}
