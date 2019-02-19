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

        Task<HttpResponseMessage> LoginUser([Header("Accept")] string applicationJson, [Body(BodySerializationMethod.Json)] string body);


        [Post("/ApiGateway/users/register")]
        Task<HttpResponseMessage> CreateUser([Header("Accept")] string applicationJson, [Body(BodySerializationMethod.Json)] string body);
    }
}
