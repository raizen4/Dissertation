using LockerApp.ServiceModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockerApp.Interfaces
{
    using System.Threading.Tasks;
    using Enums;
    using Services;
    using Microsoft.Azure.Devices.Client;
    using Models;

    public interface IIoTHub
    {

        
        Task<bool> SendMessageToLocker(LockerMessage message);

        Task<LockerMessage> GetPendingMessages();




    }
}
