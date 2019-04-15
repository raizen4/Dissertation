﻿using LockerApp.Enums;
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
        Task<ResponseData<LockerInfo>> LoginUser(string password, string email);

        Task<ResponseData<Pin>> CheckPin(string pinCode);

        Task<ResponseData<LockerInfo>> AddNewLocker(string newLockerId);

        Task<ResponseBase> AddNewActionForLocker(LockerActionEnum actionRequested, Pin pin);

        Task<ResponseBase> SendBackupBatteryNotification();

        Task<LockerMessage> GetPendingMessagesFromHub();


    }
}
