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

    class ServicesFacade:IFacade
    {

        private readonly IApiWrapper apiWrapper;

        private readonly IIoTHub iotHub;
        public ServicesFacade(IApiWrapper apiWrapper, IIoTHub iotHub)
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
        public Task<ResponseData<List<Parcel>>> GetDeliveryHistory(string profileId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<ResponseData<List<Pin>>> GetActivePins(string profileId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<ResponseData<List<Locker>>> GetLockers(string profileId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public  Task<ResponseBase> AddLockerToProfile(string profileId, Locker newLocker)
        {
            throw new NotImplementedException();
        }
    }
}
