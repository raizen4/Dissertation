using System;
using System.Collections.Generic;
using System.Text;

namespace LockerAppXamarin.Services
{
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Interfaces;
    using ModernHttpClient;
    using Refit;

    class LockerApiWrapper:ILockerPiApiWrapper
    {

        private IApiMatchingMethods _api;
        /// <summary>
        /// The HTTP client
        /// </summary>
        /// <inheritdoc />
        private HttpClient _client;

        public LockerApiWrapper()
        {
            try
            {
                InitialiseApi();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception creating ApiWrapper: {0}", ex.Message);
                throw new Exception(ex.Message, ex);
            }




        }
        /// <inheritdoc />
        public void InitialiseApi()
        {

            this._client = new HttpClient(new NativeMessageHandler())
            {

                DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", "") },
                BaseAddress = new Uri(Constants.LockerApiEndpoint),

            };


            try
            {
                this._api = RestService.For<IApiMatchingMethods>(this._client);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception creating ApiWrapper: {0}", ex.Message);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <inheritdoc />
        public async Task<HttpResponseMessage> OpenLocker()
        {

            var result = await this._api.OpenLocker();
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> GetLockerPowerStatus()
        {

            var result = await this._api.GetLockerPowerStatus();
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> CloseLocker()
        {

            var result = await this._api.CloseLocker();
            return result;
        }
    }
}
