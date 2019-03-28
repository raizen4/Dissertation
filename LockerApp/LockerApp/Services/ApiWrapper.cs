﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Services
{
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using LockerApp;
    using LockerApp.Enums;
    using LockerApp.Interfaces;
    using LockerApp.ServiceModels;
    using ModernHttpClient;
    using Newtonsoft.Json;
    using Refit;
    using Windows.Security.Credentials;
    using Windows.Storage;
    using Xamarin.Essentials;

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
                BaseAddress = new Uri(Constants.WebApiEndpoint),

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
            var jsonToSend = JsonConvert.SerializeObject(request);
            var result = await this._api.LoginUser(Constants.Headers.ContentType, jsonToSend);
            return result;
        }

        public async Task<HttpResponseMessage> CheckPin(CheckPinRequest req)
        {
            var token = "";
            var vault = new PasswordVault();
            vault.Retrieve(PreferencesEnum.Token, token);

            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);
            var jsonToSend = JsonConvert.SerializeObject(req);
            var result = await this._api.CheckPin(Constants.Headers.ContentType, jsonToSend);
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> AddNewActionForLocker(ActionRequest req)
        {
            

            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);
            var jsonToSend = JsonConvert.SerializeObject(req);
            var result = await this._api.SendRequestToLocker(Constants.Headers.ContentType, jsonToSend);
            return result;
        }

        public async Task<HttpResponseMessage> AddNewLocker(NewLockerRequest req)
        {


            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.Token);
            var jsonToSend = JsonConvert.SerializeObject(req);
            var result = await this._api.SendRequestToLocker(Constants.Headers.ContentType, jsonToSend);
            return result;
        }
    }
}
