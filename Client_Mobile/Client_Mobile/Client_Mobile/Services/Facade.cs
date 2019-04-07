using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Services
{
    using System.Net;
    using System.Threading.Tasks;
    using Enums;
    using Helpers;
    using Interfaces;
    using Microsoft.Azure.Devices.Client;
    using Models;
    using Newtonsoft.Json;
    using ServiceModels;

    class Facade:IFacade
    {

        private readonly IApiWrapper _apiWrapper;

        private readonly IIoTHub _iotHub;
        public Facade(IApiWrapper apiWrapper, IIoTHub iotHub)
        {
            this._apiWrapper = apiWrapper;
            this._iotHub = iotHub;


        }

        /// <inheritdoc />
       

        /// <inheritdoc />
        public async Task<bool> Lock()
        {
           
            var result = await this._iotHub.Lock("121321", LockerActionEnum.UserAppClose);
            if (result)
            {
                return true;
            }

            return false;


        }

        /// <inheritdoc />
        public async Task<bool> Unlock()
        {
          
                var result = await this._iotHub.Unlock("121321", LockerActionEnum.UserAppOpen);
                if (result)
                {
                    return true;
                }
            

            return false;
        }

        /// <inheritdoc />
       

        /// <inheritdoc />
    

        public async Task<Message> GetPendingMessagesFromHub()
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

        /// <inheritdoc />
        public async Task<ResponseBase> AddPinForLocker(Pin newPin)
        {
            var request = new PinRequest();
            request.Pin = newPin;

            var responseData = new ResponseBase()
            {
                IsSuccessful = false
            };
            var result = await this._apiWrapper.AddPinForLocker(request);
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
        public async Task<ResponseBase> RemovePinForLocker(Pin newPin)
        {
            var request = new PinRequest();
            request.Pin = newPin;


            var responseData = new ResponseBase()
            {
                IsSuccessful = false
            };
            var result = await this._apiWrapper.RemovePinForLocker(request);
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
        public async Task<ResponseBase> AddNewActionForLocker(LockerActionEnum type)
        {
            var request = new ActionRequest();
            request.Action = type;
            

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
        public async Task<ResponseData<User>> LoginUser(string password, string email)
        {
            var newLoginRequest=new LoginRequest();
            newLoginRequest.Password = password;
            newLoginRequest.Email = email;
            var responseData = new ResponseData<User>
            {
                IsSuccessful = false
            };

            var result = await this._apiWrapper.LoginUser(newLoginRequest);
            string content = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializedContent = JsonConvert.DeserializeObject<ResponseData<User>>(content);
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

        /// <inheritdoc />


        /// <inheritdoc />
        public async Task<ResponseData<CreatedUserInfo>> CreateUser(string email, string pass, string displayName, string phone)
        {
           var request= new RegisterRequest();
            request.DisplayName = displayName;
            request.Password = pass;
            request.Email = email;
            request.Phone = phone;
            request.DeviceId = "DeviceMobile" + PinGenerator.GeneratePin();

            var responseData = new ResponseData<CreatedUserInfo>()
            {
                IsSuccessful = false
            };
            var result = await this._apiWrapper.CreateUser(request);
            string content = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializedContent = JsonConvert.DeserializeObject<ResponseData<CreatedUserInfo>>(content);
                    if (!result.IsSuccessStatusCode || !deserializedContent.IsSuccessful)
                    {
                        responseData.IsSuccessful = false;
                        responseData.Error = "Internal Server Error";
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
                responseData.Error = "Internal Error" + result.StatusCode.ToString(); ;
                return responseData;
            }

        }

  

        /// <inheritdoc />
        public async Task<ResponseData<List<HistoryAction>>> GetDeliveryHistory()
        {
            var responseData = new ResponseData<List<HistoryAction>>
            {
                IsSuccessful = false
            };

            var result = await this._apiWrapper.GetDeliveryHistory();
            string content = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializedContent = JsonConvert.DeserializeObject<ResponseData<List<HistoryAction>>>(content);
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

        /// <inheritdoc />
        public async Task<ResponseData<List<Pin>>> GetActivePins()
        {
          
            var responseData = new ResponseData<List<Pin>>
            {
                IsSuccessful = false
            };

            var result = await this._apiWrapper.GetActivePins();
            string content = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializedContent = JsonConvert.DeserializeObject<ResponseData<List<Pin>>>(content);
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
    }
}
