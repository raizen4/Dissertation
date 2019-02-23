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
        private IApiMatchingEndpoints API;
        /// <summary>
        /// The HTTP client
        /// </summary>
        /// <inheritdoc />
        private HttpClient client;

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

            this.client = new HttpClient(new NativeMessageHandler())
            {

                DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", "") },
                BaseAddress = new Uri(Constants.WebApiEndpoint),

            };


            try
            {
                this.API = RestService.For<IApiMatchingEndpoints>(this.client);
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
            var result = await this.API.LoginUser(Constants.Headers.ContentType, jsonToSend);
            return result;
        }

        public async Task<HttpResponseMessage> CreateUser(RegisterRequest request)
        {

            var jsonToSend = JsonConvert.SerializeObject(request);
            var result = await this.API.CreateUser(Constants.Headers.ContentType, jsonToSend);
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> GetDeliveryHistory()
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.CurrentLoggedInUser.Token);
            
            var result = await this.API.GetDeliveryHistory();
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> GetActivePins()
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.CurrentLoggedInUser.Token);
           
            var result = await this.API.GetActivePins();
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> AddPinForLocker(PinRequest req)
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.CurrentLoggedInUser.Token);
            var jsonToSend = JsonConvert.SerializeObject(req);
            var result = await this.API.AddPinForLocker(Constants.Headers.ContentType, jsonToSend);
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> RemovePinForLocker(PinRequest req)
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.CurrentLoggedInUser.Token);
            var jsonToSend = JsonConvert.SerializeObject(req);
            var result = await this.API.RemovePinForLocker(Constants.Headers.ContentType, jsonToSend);
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> AddNewActionForLocker(ActionRequest req)
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.CurrentLoggedInUser.Token);
            var jsonToSend = JsonConvert.SerializeObject(req);
            var result = await this.API.AddNewActionForLocker(Constants.Headers.ContentType, jsonToSend);
            return result;
        }
    }
}
