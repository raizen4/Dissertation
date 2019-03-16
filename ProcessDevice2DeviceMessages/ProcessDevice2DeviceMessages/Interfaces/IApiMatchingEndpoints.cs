using Refit;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProcessDevice2DeviceMessages.Services
{
    internal interface IApiMatchingEndpoints
    {
        [Put("/UserApi/users/AddNewActionForLocker")]
        Task<HttpResponseMessage> AddNewActionForLocker([Header("Accept")] string applicationJson, [Body(BodySerializationMethod.Json)] string body);

    }
}