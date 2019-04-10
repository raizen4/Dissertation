using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Services
{
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Enums;
    using Interfaces;
    using ModernHttpClient;
    using Newtonsoft.Json;
    using Refit;
    using ServiceModels;
    using Xamarin.Essentials;

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
            var content = new StringContent(jsonToSend, Encoding.UTF8, Constants.Headers.ContentType);
            var result = await this._api.LoginUser(Constants.Headers.ContentType, content);
            return result;
        }

        public async Task<HttpResponseMessage> CreateUser(RegisterRequest request)
        {
            var jsonToSend = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonToSend, Encoding.UTF8, Constants.Headers.ContentType);
            var result = await this._api.CreateUser(Constants.Headers.ContentType, content);
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> GetDeliveryHistory()
        {
            var token = Constants.Token;
            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);         
            var result = await this._api.GetDeliveryHistory();
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> GetActivePins()
        {
            var token = Constants.Token;
            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);       
            var result = await this._api.GetActivePins();
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> AddPinForLocker(PinRequest req)
        {
            var token = Constants.Token;
            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var jsonToSend = JsonConvert.SerializeObject(req);
            var content = new StringContent(jsonToSend, Encoding.UTF8, Constants.Headers.ContentType);
            var result = await this._api.AddPinForLocker(Constants.Headers.ContentType, content);
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> RemovePinForLocker(PinRequest req)
        {
            var token = Constants.Token;
            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var jsonToSend = JsonConvert.SerializeObject(req);
            var content = new StringContent(jsonToSend, Encoding.UTF8, Constants.Headers.ContentType);
            var result = await this._api.RemovePinForLocker(Constants.Headers.ContentType, content);
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> AddNewActionForLocker(ActionRequest req)
        {
            var token = Constants.Token;
            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var jsonToSend = JsonConvert.SerializeObject(req);
            var content = new StringContent(jsonToSend, Encoding.UTF8, Constants.Headers.ContentType);
            var result = await this._api.AddNewActionForLocker(Constants.Headers.ContentType, content);
            return result;
        }
    }
}
