using Client_Mobile.Enums;
using Client_Mobile.Interfaces;
using Client_Mobile.ServiceModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Willezone.Azure.WebJobs.Extensions.DependencyInjection;

namespace ProcessDevice2DeviceMessages.Services
{
    class Facade : IFacade
    {
        private readonly IApiWrapper _apiWrapper;
        

      
        public Facade([Inject]IApiWrapper apiWrapper )
        {
            this._apiWrapper = apiWrapper;
           


        }
        public async Task<ResponseBase> AddNewActionForLocker(LockerActionEnum type, string token)
        {
            var request = new ActionRequest();
            request.Action = type;
            request.Token = token;

            var responseData = new ResponseBase()
            {
                IsSuccessful = false
            };
            var result = await this._apiWrapper.AddNewActionForLockerAsync(request);
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
    }
}
