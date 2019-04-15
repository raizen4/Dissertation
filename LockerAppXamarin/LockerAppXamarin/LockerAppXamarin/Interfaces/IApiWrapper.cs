﻿using LockerAppXamarin.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LockerAppXamarin.Interfaces
{
    using ServiceModels;

    interface IApiWrapper
    {
        void InitialiseApi();

        Task<HttpResponseMessage> LoginUser(LoginRequest request);

        Task<HttpResponseMessage> AddNewActionForLocker(ActionRequest req);

        Task<HttpResponseMessage> CheckPin(CheckPinRequest req);

        Task<HttpResponseMessage> AddNewLocker(NewLockerRequest req);


        Task<HttpResponseMessage> SendBackupBatteryNotification();

      



    }
}