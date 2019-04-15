using LockerAppXamarin.Enums;
using LockerAppXamarin.Models;
using LockerAppXamarin.ServiceModels;
using LockerAppXamarin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockerAppXamarin.Interfaces
{
    using Enums;
    using Models;
    using ServiceModels;

    public interface IFacade
    {
        Task<ResponseData<LockerInfo>> LoginUser(string password, string email);

        Task<ResponseData<Pin>> CheckPin(string pinCode);

        Task<ResponseData<LockerInfo>> AddNewLocker(string newLockerId);

        Task<ResponseBase> AddNewActionForLocker(LockerActionEnum actionRequested, Pin pin);

        Task<ResponseBase> SendBackupBatteryNotification();

        Task<ResponseBase> OpenLocker();

        Task<ResponseData<PowerTypeEnum?>> GetLockerPowerStatus();
        Task<ResponseBase> CloseLocker();

        Task<LockerMessage> GetPendingMessagesFromHub();


    }
}
