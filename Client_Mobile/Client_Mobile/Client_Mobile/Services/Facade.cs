using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Facade
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

        private readonly IApiWrapper apiWrapper;

        private readonly IIoTHub iotHub;
        public Facade(IApiWrapper apiWrapper, IIoTHub iotHub)
        {
            this.apiWrapper = apiWrapper;
            this.iotHub = iotHub;


        }

        /// <inheritdoc />
       

        /// <inheritdoc />
        public async Task<bool> Lock()
        {
           
            var result = await this.iotHub.Lock("121321", LockerActionEnum.Close);
            if (result)
            {
                return true;
            }

            return false;


        }

        /// <inheritdoc />
        public async Task<bool> Unlock()
        {
          
                var result = await this.iotHub.Unlock("121321", LockerActionEnum.Open);
                if (result)
                {
                    return true;
                }
            

            return false;
        }

        /// <inheritdoc />
        public async Task<bool> SendPinToLocker(Pin newPin)
        {
                var result = await this.iotHub.SendPinToLocker("121321", LockerActionEnum.NewPinGenerated,
                   newPin);
                if (result)
                {
                    return true;
                }

            return false;
        }

        /// <inheritdoc />
        public Task<ResponseData<List<Pin>>> GetActivePins()
        {
            throw new NotImplementedException();
        }

        public async Task<Message> GetPendingMessagesFromHub()
        {
            try
            {
                var result = await this.iotHub.GetPendingMessages();
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
            var result = await this.apiWrapper.AddPinForLocker(request);
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
            var result = await this.apiWrapper.RemovePinForLocker(request);
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
            var result = await this.apiWrapper.AddNewActionForLocker(request);
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

            var result = await this.apiWrapper.LoginUser(newLoginRequest);
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
        public async Task<ResponseBase> CreateUser(string email, string pass, string profileId)
        {
           var request=new RegisterRequest();
            request.ProfileId = profileId;
            request.Password = pass;
            request.Email = email;

            var responseData = new ResponseBase()
            {
                IsSuccessful = false
            };
            var result = await this.apiWrapper.CreateUser(request);
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
        public async Task<ResponseData<List<HistoryAction>>> GetDeliveryHistory()
        {
            var responseData = new ResponseData<List<HistoryAction>>
            {
                IsSuccessful = false
            };

            var result = await this.apiWrapper.GetDeliveryHistory();
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
        public async Task<ResponseData<List<Pin>>> GetActivePins(string profileId)
        {
          
            var responseData = new ResponseData<List<Pin>>
            {
                IsSuccessful = false
            };

            var result = await this.apiWrapper.GetActivePins();
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
