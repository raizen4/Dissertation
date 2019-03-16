using Client_Mobile.Interfaces;
using ModernHttpClient;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDevice2DeviceMessages.Services
{
    class ApiWrapper : IApiWrapper
    {
        private IApiMatchingEndpoints _api;
        /// <summary>
        /// The HTTP client
        /// </summary>
        /// <inheritdoc />
        private HttpClient _client;

        public ApiWrapper()
        {
            try
            {
                InitialiseApi();
                Debug.WriteLine("Injected wrapper up and running");

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception creating ApiWrapper: {0}", ex.Message);
                throw new Exception(ex.Message, ex);
            }




        }
        public void InitialiseApi()
        {

            this._client = new HttpClient(new NativeMessageHandler())
            {

                DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", "") },
                BaseAddress = new Uri(Constants.UserApiEndpoint),

            };


            try
            {
                this._api = RestService.For<IApiMatchingEndpoints>(this._client);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception creating ApiWrapper: {0}", ex.Message);
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<HttpResponseMessage> AddNewActionForLockerAsync(ActionRequest req)
        {
           
            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", req.Token);
            var jsonToSend = JsonConvert.SerializeObject(req);
            var result = await this._api.AddNewActionForLocker(Constants.Headers.ContentType, jsonToSend);
            return result;
        }


    }
}
