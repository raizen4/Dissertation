using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Interfaces
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using ServiceModels;

    interface IApiWrapper
    {
        void InitialiseApi();

        Task<HttpResponseMessage> LoginUser(LoginRequest request);


        Task<HttpResponseMessage> CreateUser(RegisterRequest request);

        Task<HttpResponseMessage> GetDeliveryHistory();

        Task<HttpResponseMessage> GetActivePins();

        Task<HttpResponseMessage> GetLockers();

        Task<HttpResponseMessage> AddLockerToProfile();

    }
}
