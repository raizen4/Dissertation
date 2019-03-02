using Client_Mobile.ServiceModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Interfaces
{
    using System.Threading.Tasks;
    using Enums;
    using Microsoft.Azure.Devices.Client;
    using Models;

    public interface IIoTHub
    {

        Task<bool> CheckConnection();
        Task<bool> Lock(string deviceId, LockerActionEnum action);
        Task<bool> Unlock(string deviceId, LockerActionEnum action);
     

        Task<Message> GetPendingMessages();




    }
}
