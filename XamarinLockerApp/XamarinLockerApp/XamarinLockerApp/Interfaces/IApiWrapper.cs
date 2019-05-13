namespace XamarinLockerApp.Interfaces
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using ServiceModels;

    interface IApiWrapper
    {
        void InitialiseApi();

        Task<HttpResponseMessage> LoginUser(LoginRequest request);

        Task<HttpResponseMessage> AddNewActionForLocker(ActionRequest req);

        Task<HttpResponseMessage> CheckPin(CheckPinRequest req);

        Task<HttpResponseMessage> AddNewLocker(NewLockerRequest req);


        Task<HttpResponseMessage> SendPowerStatusChangedNotification(NewPowerStatusRequest req);

      



    }
}
