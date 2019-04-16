namespace XamarinLockerApp.Interfaces
{
    using System.Threading.Tasks;
    using ServiceModels;

    public interface IIoTHub
    {

        
        Task<bool> SendMessageToLocker(LockerMessage message);

        Task<LockerMessage> GetPendingMessages();




    }
}
