using LockerApp.Enums;
using LockerApp.Models;
using LockerApp.ServiceModels;
using LockerApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockerApp.Interfaces
{
    public interface IFacade
    {
        Task<ResponseData<User>> LoginUser(string password, string email);

        Task<ResponseData<Pin>> CheckPin(Pin pin);

        Task<ResponseData<Locker>> AddNewLocker(string newLockerId);

        Task<ResponseBase> AddNewActionForLocker(LockerActionRequestsEnum actionRequested, Pin pin);

        Task<LockerMessage> GetPendingMessagesFromHub();


    }
}
