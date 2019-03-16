using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LockerApp.Interfaces
{
    interface IApiMatchingMethods
    {

        [Post("/UserApi/users/login")]
        Task<HttpResponseMessage> LoginUser([Header("Accept")] string applicationJson, [Body(BodySerializationMethod.Json)] string body);

        [Post("/UserApi/users/AddActionToLocker")]
        Task<HttpResponseMessage> SendRequestToLocker([Header("Accept")] string applicationJson, [Body(BodySerializationMethod.Json)] string body);




        [Post("/UserApi/users/CheckPin")]
        Task<HttpResponseMessage> CheckPin([Header("Accept")] string applicationJson, [Body(BodySerializationMethod.Json)] string body);
    }
}
