namespace XamarinLockerApp.Services
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Interfaces;
    using ModernHttpClient;
    using Newtonsoft.Json;
    using Refit;
    using ServiceModels;

    class ApiWrapper:IApiWrapper
    {
        private IApiMatchingMethods _api;
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
                BaseAddress = new Uri(Constants.UserWebApiEndpoint),

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
    

        /// <inheritdoc />
        public async Task<HttpResponseMessage> LoginUser(LoginRequest request)
        {
            request.LoginType = "LOCKER";
            var jsonToSend = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonToSend, Encoding.UTF8, Constants.Headers.ContentType);
            var result = await this._api.LoginUser(Constants.Headers.ContentType, content);
            return result;
        }

        public async Task<HttpResponseMessage> CheckPin(CheckPinRequest req)
        {
           

            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);
            var jsonToSend = JsonConvert.SerializeObject(req);
            var content = new StringContent(jsonToSend, Encoding.UTF8, Constants.Headers.ContentType);
            var result = await this._api.CheckPin(Constants.Headers.ContentType, content);
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> AddNewActionForLocker(ActionRequest req)
        {
            

            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);
            var jsonToSend = JsonConvert.SerializeObject(req);
            var content = new StringContent(jsonToSend, Encoding.UTF8, Constants.Headers.ContentType);
            var result = await this._api.SendRequestToLocker(Constants.Headers.ContentType, content);
            return result;
        }

        public async Task<HttpResponseMessage> AddNewLocker(NewLockerRequest req)
        {


            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);
            var jsonToSend = JsonConvert.SerializeObject(req);
            var content = new StringContent(jsonToSend, Encoding.UTF8, Constants.Headers.ContentType);
            var result = await this._api.AddNewLocker(Constants.Headers.ContentType, content);
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> SendPowerStatusChangedNotification()
        {
            var result = await this._api.SendPowerStatusChangedNotification();
            return result;
        }

      
    }
    }
