namespace XamarinLockerApp.Interfaces
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Refit;

    interface IApiMatchingMethods
    {

        [Post("/UserApi/users/login")]
        Task<HttpResponseMessage> LoginUser([Header("Accept")] string applicationJson, [Body(BodySerializationMethod.Json)] StringContent body);

        [Put("/UserApi/users/AddNewActionForLocker")]
        Task<HttpResponseMessage> SendRequestToLocker([Header("Accept")] string applicationJson, [Body(BodySerializationMethod.Json)] StringContent body);

        [Post("/UserApi/users/CreateLocker")]
        Task<HttpResponseMessage> AddNewLocker([Header("Accept")] string applicationJson, [Body(BodySerializationMethod.Json)] StringContent body);



        [Put("/UserApi/users/CheckPin")]
        Task<HttpResponseMessage> CheckPin([Header("Accept")] string applicationJson, [Body(BodySerializationMethod.Json)] StringContent body);




        [Put("/api/Locker/OpenLocker")]
        Task<HttpResponseMessage> OpenLocker();




        [Put("/api/Locker/CloseLocker")]
        Task<HttpResponseMessage> CloseLocker();


        [Get("/api/Locker/GetPowerStatus")]
        Task<HttpResponseMessage> GetLockerPowerStatus();



        [Put("/UserApi/users/SendPowerStatusChanged")]
        Task<HttpResponseMessage> SendPowerStatusChangedNotification([Header("Accept")] string applicationJson, [Body(BodySerializationMethod.Json)] StringContent body);
    }
}
