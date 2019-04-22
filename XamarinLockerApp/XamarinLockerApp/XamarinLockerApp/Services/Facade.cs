namespace XamarinLockerApp.Services
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Enums;
    using Interfaces;
    using Models;
    using Newtonsoft.Json;
    using ServiceModels;

    class Facade:IFacade
    {

        private  IApiWrapper _apiWrapper;
        private ILockerPiApiWrapper _lockerWrapper;
        private  IIoTHub _iotHub;
        public Facade(IApiWrapper apiWrapper, IIoTHub iotHub, ILockerPiApiWrapper lockerWrapper)
        {
            this._apiWrapper = apiWrapper;
            this._iotHub = iotHub;
            this._lockerWrapper = lockerWrapper;

        }


        public async Task<ResponseData<LockerInfo>> AddNewLocker(string newLockerId)
        {
            var newLockerRequest = new NewLockerRequest();
            newLockerRequest.LockerId = newLockerId;
;
            var responseData = new ResponseData<LockerInfo>
            {
                HasBeenSuccessful = false
            };

            var result = await this._apiWrapper.AddNewLocker(newLockerRequest);
            string content = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializedContent = JsonConvert.DeserializeObject<ResponseData<LockerInfo>>(content);
                    if (deserializedContent.HasBeenSuccessful == false)
                    {
                        responseData.HasBeenSuccessful = false;
                        responseData.Error = deserializedContent.Error;
                        responseData.Content = null;
                        return responseData;
                    }

                    responseData.HasBeenSuccessful = true;
                    responseData.Error = null;
                    responseData.Content = deserializedContent.Content;
                    return responseData;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    responseData.HasBeenSuccessful = false;
                    responseData.Error = "Deserialization Error";
                    responseData.Content = null;
                    return responseData;
                }

            }
            else
            {
                responseData.HasBeenSuccessful = false;
                responseData.Error = "Internal server error";
                responseData.Content = null;
                return responseData;
            }

        }


        public async Task<ResponseData<LockerInfo>> LoginUser(string password, string email)
        {
            var newLoginRequest = new LoginRequest();
            newLoginRequest.Password = password;
            newLoginRequest.Email = email;
            var responseData = new ResponseData<LockerInfo>
            {
                HasBeenSuccessful = false
            };

            var result = await this._apiWrapper.LoginUser(newLoginRequest);
            string content = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializedContent = JsonConvert.DeserializeObject<ResponseData<LockerInfo>>(content);
                    if (deserializedContent.HasBeenSuccessful == false)
                    {
                        responseData.HasBeenSuccessful = false;
                        responseData.Error = deserializedContent.Error;
                        responseData.Content = null;
                        return responseData;
                    }

                    responseData.HasBeenSuccessful = true;
                    responseData.Error = null;
                    responseData.Content = deserializedContent.Content;
                    return responseData;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    responseData.HasBeenSuccessful = false;
                    responseData.Error = "Deserialization Error";
                    responseData.Content = null;
                    return responseData;
                }

            }
            else
            {
                responseData.HasBeenSuccessful = false;
                responseData.Error = "Internal server error";
                responseData.Content = null;
                return responseData;
            }

        }

        public async Task<ResponseBase> AddNewActionForLocker(LockerActionEnum actionRequested, Pin pin)
        {


            var request = new ActionRequest();
            request.ActionType = actionRequested;
            request.Pin = pin;

            var responseData = new ResponseBase()
            {
                HasBeenSuccessful = false
            };
            var result = await this._apiWrapper.AddNewActionForLocker(request);
            string content = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializedContent = JsonConvert.DeserializeObject<ResponseBase>(content);
                    if ( !deserializedContent.HasBeenSuccessful)
                    {
                        responseData.HasBeenSuccessful = false;
                        responseData.Error = "Internal Server Error";
                        return responseData;
                    }

                    responseData.HasBeenSuccessful = true;
                    responseData.Error = null;
                    return responseData;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    responseData.HasBeenSuccessful = false;
                    responseData.Error = "Deserialization Error";
                    return responseData;
                }
            }
            else
            {
                responseData.HasBeenSuccessful = false;
                responseData.Error = "Internal Error" + result.StatusCode.ToString(); ;
                return responseData;
            }
        }

        /// <inheritdoc />
        public async Task<ResponseBase> SendPowerStatusChangedNotification(PowerTypeEnum powerStatus)
        {
            var responseData = new ResponseBase()
            {
                HasBeenSuccessful = false
            };

            var request=new NewPowerStatusRequest();
            request.PowerStatus = powerStatus;
            var result = await this._apiWrapper.SendPowerStatusChangedNotification(request);
            string content = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializedContent = JsonConvert.DeserializeObject<ResponseBase>(content);
                    if (!deserializedContent.HasBeenSuccessful)
                    {
                        responseData.HasBeenSuccessful = false;
                        responseData.Error = "Internal Server Error";
                        return responseData;
                    }

                    responseData.HasBeenSuccessful = true;
                    responseData.Error = null;
                    return responseData;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    responseData.HasBeenSuccessful = false;
                    responseData.Error = "Deserialization Error";
                    return responseData;
                }
            }
            else
            {
                responseData.HasBeenSuccessful = false;
                responseData.Error = "Internal Error" + result.StatusCode.ToString(); ;
                return responseData;
            }
        }

        /// <inheritdoc />
    

        /// <inheritdoc />
        public async Task<ResponseBase> OpenLocker()
        {

            var responseData = new ResponseBase()
            {
                HasBeenSuccessful = false
            };
            var result = await this._lockerWrapper.OpenLocker();
            string content = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializedContent = JsonConvert.DeserializeObject<ResponseBase>(content);
                    if (!deserializedContent.HasBeenSuccessful)
                    {
                        responseData.HasBeenSuccessful = false;
                        responseData.Error = "Internal Server Error";
                        return responseData;
                    }

                    responseData.HasBeenSuccessful = true;
                    responseData.Error = null;
                    return responseData;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    responseData.HasBeenSuccessful = false;
                    responseData.Error = "Deserialization Error";
                    return responseData;
                }
            }
            else
            {
                responseData.HasBeenSuccessful = false;
                responseData.Error = "Internal Error" + result.StatusCode.ToString(); ;
                return responseData;
            }
        }

        /// <inheritdoc />
        public async Task<ResponseData<PowerTypeEnum?>> GetLockerPowerStatus()
        {
          
            var responseData = new ResponseData<PowerTypeEnum?>
            {
                HasBeenSuccessful = false
            };

            var result = await this._lockerWrapper.GetLockerPowerStatus();
            string content = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializedContent = JsonConvert.DeserializeObject<ResponseData<PowerTypeEnum>>(content);
                    if (deserializedContent.HasBeenSuccessful == false)
                    {
                        responseData.HasBeenSuccessful = false;
                        responseData.Error = deserializedContent.Error;
                        responseData.Content = null;
                        return responseData;
                    }

                    responseData.HasBeenSuccessful = true;
                    responseData.Error = null;
                    responseData.Content = deserializedContent.Content;
                    return responseData;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    responseData.HasBeenSuccessful = false;
                    responseData.Error = "Deserialization Error";
                    responseData.Content = null;
                    return responseData;
                }

            }
            else
            {
                responseData.HasBeenSuccessful = false;
                responseData.Error = "Internal server error";
                responseData.Content = null;
                return responseData;
            }
        }

        /// <inheritdoc />
        public async Task<ResponseBase> CloseLocker()
        {

            var responseData = new ResponseBase()
            {
                HasBeenSuccessful = false
            };
            var result = await this._lockerWrapper.CloseLocker();
            string content = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializedContent = JsonConvert.DeserializeObject<ResponseBase>(content);
                    if (!deserializedContent.HasBeenSuccessful)
                    {
                        responseData.HasBeenSuccessful = false;
                        responseData.Error = "Internal Server Error";
                        return responseData;
                    }

                    responseData.HasBeenSuccessful = true;
                    responseData.Error = null;
                    return responseData;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    responseData.HasBeenSuccessful = false;
                    responseData.Error = "Deserialization Error";
                    return responseData;
                }
            }
            else
            {
                responseData.HasBeenSuccessful = false;
                responseData.Error = "Internal Error" + result.StatusCode.ToString(); ;
                return responseData;
            }
        }

        public async Task<ResponseData<Pin>> CheckPin(string pin)
        {
            var request = new CheckPinRequest();
            request.PinCode = pin;

            var responseData = new ResponseData<Pin>()
            {
                HasBeenSuccessful = false
            };
            var result = await this._apiWrapper.CheckPin(request);
            string content = await result.Content.ReadAsStringAsync();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var deserializedContent = JsonConvert.DeserializeObject<ResponseData<Pin>>(content);
                    if (!deserializedContent.HasBeenSuccessful)
                    {
                        responseData.HasBeenSuccessful = false;
                        responseData.Content = null;
                        responseData.Error = "Internal Server Error";
                        return responseData;
                    }

                    responseData.HasBeenSuccessful = true;
                    responseData.Content = deserializedContent.Content;
                    responseData.Error = null;
                    return responseData;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    responseData.HasBeenSuccessful = false;
                    responseData.Content = null;
                    responseData.Error = "Deserialization Error";
                    return responseData;
                }
            }
            else
            {
                responseData.HasBeenSuccessful = false;
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
