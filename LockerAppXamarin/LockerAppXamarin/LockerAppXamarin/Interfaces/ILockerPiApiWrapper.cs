using System;
using System.Collections.Generic;
using System.Text;

namespace LockerAppXamarin.Interfaces
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
