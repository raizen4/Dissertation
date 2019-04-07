using System;
using System.Collections.Generic;
using System.Text;

namespace LockerApp.Services
{
    using System.Net;
    using System.Threading.Tasks;
    using Enums;
    using Interfaces;
    using Microsoft.Azure.Devices.Client;
    using Models;
    using Newtonsoft.Json;
    using ServiceModels;

    class Facade:IFacade
    {

        private  IApiWrapper _apiWrapper;

        private  IIoTHub _iotHub;
        public Facade(IApiWrapper apiWrapper, IIoTHub iotHub)
        {
            this._apiWrapper = apiWrapper;
            this._iotHub = iotHub;


        }


        public async Task<ResponseData<Locker>> AddNewLocker(string newLockerId)
        {
            var newLockerRequest = new NewLockerRequest();
            newLockerRequest.NewLockerId = newLockerId;
;
            var responseData = new ResponseData<Locker>
            {
                IsSuccessful = false
            };

            var result = await this._apiWrapper.AddNewLocker(newLockerRequest);
            string content = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializedContent = JsonConvert.DeserializeObject<ResponseData<Locker>>(content);
                    if (deserializedContent.IsSuccessful == false)
                    {
                        responseData.IsSuccessful = false;
                        responseData.Error = deserializedContent.Error;
                        responseData.Content = null;
                        return responseData;
                    }

                    responseData.IsSuccessful = true;
                    responseData.Error = null;
                    responseData.Content = deserializedContent.Content;
                    return responseData;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    responseData.IsSuccessful = false;
                    responseData.Error = "Deserialization Error";
                    responseData.Content = null;
                    return responseData;
                }

            }
            else
            {
                responseData.IsSuccessful = false;
                responseData.Error = "Internal server error";
                responseData.Content = null;
                return responseData;
            }

        }


        public async Task<ResponseData<Locker>> LoginUser(string password, string email)
        {
            var newLoginRequest = new LoginRequest();
            newLoginRequest.Password = password;
            newLoginRequest.Email = email;
            var responseData = new ResponseData<Locker>
            {
                IsSuccessful = false
            };

            var result = await this._apiWrapper.LoginUser(newLoginRequest);
            string content = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializedContent = JsonConvert.DeserializeObject<ResponseData<Locker>>(content);
                    if (deserializedContent.IsSuccessful == false)
                    {
                        responseData.IsSuccessful = false;
                        responseData.Error = deserializedContent.Error;
                        responseData.Content = null;
                        return responseData;
                    }

                    responseData.IsSuccessful = true;
                    responseData.Error = null;
                    responseData.Content = deserializedContent.Content;
                    return responseData;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    responseData.IsSuccessful = false;
                    responseData.Error = "Deserialization Error";
                    responseData.Content = null;
                    return responseData;
                }

            }
            else
            {
                responseData.IsSuccessful = false;
                responseData.Error = "Internal server error";
                responseData.Content = null;
                return responseData;
            }

        }

        public async Task<ResponseBase> AddNewActionForLocker(LockerActionRequestsEnum actionRequested, Pin pin)
        {


            var request = new ActionRequest();
            request.Action = actionRequested;
            request.Pin = pin;

            var responseData = new ResponseBase()
            {
                IsSuccessful = false
            };
            var result = await this._apiWrapper.AddNewActionForLocker(request);
            string content = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializedContent = JsonConvert.DeserializeObject<ResponseBase>(content);
                    if (!result.IsSuccessStatusCode || !deserializedContent.IsSuccessful)
                    {
                        responseData.IsSuccessful = false;
                        responseData.Error = "Internal Server Error";
                        return responseData;
                    }

                    responseData.IsSuccessful = true;
                    responseData.Error = null;
                    return responseData;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    responseData.IsSuccessful = false;
                    responseData.Error = "Deserialization Error";
                    return responseData;
                }
            }
            else
            {
                responseData.IsSuccessful = false;
                responseData.Error = "Internal Error" + result.StatusCode.ToString(); ;
                return responseData;
            }
        }

        /// <inheritdoc />
        public async Task<ResponseBase> SendBackupBatteryNotification()
        {

            
            var responseData = new ResponseBase()
            {
                IsSuccessful = false
            };
            var result = await this._apiWrapper.SendBackupBatteryNotification();
            string content = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializedContent = JsonConvert.DeserializeObject<ResponseBase>(content);
                    if (!result.IsSuccessStatusCode || !deserializedContent.IsSuccessful)
                    {
                        responseData.IsSuccessful = false;
                        responseData.Error = "Internal Server Error";
                        return responseData;
                    }

                    responseData.IsSuccessful = true;
                    responseData.Error = null;
                    return responseData;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    responseData.IsSuccessful = false;
                    responseData.Error = "Deserialization Error";
                    return responseData;
                }
            }
            else
            {
                responseData.IsSuccessful = false;
                responseData.Error = "Internal Error" + result.StatusCode.ToString(); ;
                return responseData;
            }
        }

        public async Task<ResponseData<Pin>> CheckPin(Pin pin)
        {
            var request = new CheckPinRequest();
            request.PinToBeChecked = pin;

            var responseData = new ResponseData<Pin>()
            {
                IsSuccessful = false
            };
            var result = await this._apiWrapper.CheckPin(request);
            string content = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializedContent = JsonConvert.DeserializeObject<ResponseData<Pin>>(content);
                    if (!result.IsSuccessStatusCode || !deserializedContent.IsSuccessful)
                    {
                        responseData.IsSuccessful = false;
                        responseData.Content = null;
                        responseData.Error = "Internal Server Error";
                        return responseData;
                    }

                    responseData.IsSuccessful = true;
                    responseData.Content = deserializedContent.Content;
                    responseData.Error = null;
                    return responseData;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    responseData.IsSuccessful = false;
                    responseData.Content = null;
                    responseData.Error = "Deserialization Error";
                    return responseData;
                }
            }
            else
            {
                responseData.IsSuccessful = false;
                responseData.Error = "Internal Error" + result.StatusCode.ToString(); ;
                return responseData;
            }
        }

        public  async Task<LockerMessage> GetPendingMessagesFromHub()
        {
            try
            {
                var result = await this._iotHub.GetPendingMessages();
                if (result != null)
                {
                    return result;
                }

                return null;

            }
            catch (Exception e)
            {
                return null;
            }


        }

    }
}
