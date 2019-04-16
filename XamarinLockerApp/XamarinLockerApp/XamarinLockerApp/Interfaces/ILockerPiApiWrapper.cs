namespace XamarinLockerApp.Interfaces
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface ILockerPiApiWrapper
    {
        void InitialiseApi();

        Task<HttpResponseMessage> OpenLocker();

        Task<HttpResponseMessage> GetLockerPowerStatus();

        Task<HttpResponseMessage> CloseLocker();
    }
}
