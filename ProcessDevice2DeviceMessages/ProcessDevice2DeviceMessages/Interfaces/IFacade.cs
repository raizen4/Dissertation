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

        

        Task<ResponseBase> AddNewActionForLocker(LockerActionEnum type,string token);



    }
}
