namespace XamarinLockerApp.Interfaces
{
    using System.Threading.Tasks;
    using Enums;
    using Models;
    using ServiceModels;

    public interface IFacade
    {
        Task<ResponseData<LockerInfo>> LoginUser(string password, string email);

        Task<ResponseData<Pin>> CheckPin(string pinCode);

        Task<ResponseData<LockerInfo>> AddNewLocker(string newLockerId);

        Task<ResponseBase> AddNewActionForLocker(LockerActionEnum actionRequested, Pin pin);

        Task<ResponseBase> SendPowerStatusChangedNotification(PowerTypeEnum newPowerStatus);


        Task<ResponseData<PowerTypeEnum?>> GetLockerPowerStatus();
        Task<ResponseBase> CloseLocker();

        Task<ResponseBase> OpenLocker();

        Task<LockerMessage> GetPendingMessagesFromHub();


    }
}
