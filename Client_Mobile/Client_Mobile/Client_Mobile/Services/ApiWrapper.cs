using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Services
{
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Interfaces;
    using ModernHttpClient;
    using Newtonsoft.Json;
    using Refit;
    using ServiceModels;

    class ApiWrapper:IApiWrapper
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
                BaseAddress = new Uri(Constants.WebApiEndpoint),

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

        /// <inheritdoc />
    

        /// <inheritdoc />
        public async Task<HttpResponseMessage> LoginUser(LoginRequest request)
        {
            var jsonToSend = JsonConvert.SerializeObject(request);
            var result = await this._api.LoginUser(Constants.Headers.ContentType, jsonToSend);
            return result;
        }

        public async Task<HttpResponseMessage> CreateUser(RegisterRequest request)
        {

            var jsonToSend = JsonConvert.SerializeObject(request);
            var result = await this._api.CreateUser(Constants.Headers.ContentType, jsonToSend);
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> GetDeliveryHistory()
        {
            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.CurrentLoggedInUser.Token);
            
            var result = await this._api.GetDeliveryHistory();
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> GetActivePins()
        {
            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.CurrentLoggedInUser.Token);
           
            var result = await this._api.GetActivePins();
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> AddPinForLocker(PinRequest req)
        {
            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.CurrentLoggedInUser.Token);
            var jsonToSend = JsonConvert.SerializeObject(req);
            var result = await this._api.AddPinForLocker(Constants.Headers.ContentType, jsonToSend);
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> RemovePinForLocker(PinRequest req)
        {
            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.CurrentLoggedInUser.Token);
            var jsonToSend = JsonConvert.SerializeObject(req);
            var result = await this._api.RemovePinForLocker(Constants.Headers.ContentType, jsonToSend);
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> AddNewActionForLocker(ActionRequest req)
        {
            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.CurrentLoggedInUser.Token);
            var jsonToSend = JsonConvert.SerializeObject(req);
            var result = await this._api.AddNewActionForLocker(Constants.Headers.ContentType, jsonToSend);
            return result;
        }
    }
}
