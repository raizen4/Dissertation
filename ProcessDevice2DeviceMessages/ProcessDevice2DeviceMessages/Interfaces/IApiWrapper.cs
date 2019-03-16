using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Interfaces
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Enums;
    using Models;
    using ServiceModels;

    public interface IApiWrapper
    {
        void InitialiseApi();

      
        Task<HttpResponseMessage> AddNewActionForLockerAsync(ActionRequest req);


    }
}
