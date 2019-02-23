using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Interfaces
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Refit;

    interface IApiMatchingEndpoints
    {
        [Post("/UserApi/users/login")]
        Task<HttpResponseMessage> LoginUser([Header("Accept")] string applicationJson, [Body(BodySerializationMethod.Json)] string body);


        [Post("/UserApi/users/register")]
        Task<HttpResponseMessage> CreateUser([Header("Accept")] string applicationJson, [Body(BodySerializationMethod.Json)] string body);

        [Get("/UserApi/users/GetDeliveryHistory")]
        Task<HttpResponseMessage> GetDeliveryHistory();

        [Get("/UserApi/users/GetActivePins")]
        Task<HttpResponseMessage> GetActivePins();



        [Put("/UserApi/users/AddPinForLocker")]
        Task<HttpResponseMessage> AddPinForLocker([Header("Accept")] string applicationJson, [Body(BodySerializationMethod.Json)] string body);

        [Put("/UserApi/users/RemovePinForLocker")]
        Task<HttpResponseMessage> RemovePinForLocker([Header("Accept")] string applicationJson, [Body(BodySerializationMethod.Json)] string body);

        [Put("/UserApi/users/AddNewActionForLocker")]
        Task<HttpResponseMessage> AddNewActionForLocker([Header("Accept")] string applicationJson, [Body(BodySerializationMethod.Json)] string body);
    }
}
