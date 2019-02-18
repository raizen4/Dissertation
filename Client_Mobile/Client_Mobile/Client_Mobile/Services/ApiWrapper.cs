using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Interfaces;
    using ServiceModels;

    class ApiWrapper:IApiWrapper
    {
        /// <inheritdoc />
        public void InitialiseApi()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<HttpResponseMessage> LoginUser()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<HttpResponseMessage> CreateUser()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<HttpResponseMessage> GetDeliveryHistory()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<HttpResponseMessage> GetActivePins()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<HttpResponseMessage> GetLockers()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<HttpResponseMessage> AddLockerToProfile()
        {
            throw new NotImplementedException();
        }
    }
}
