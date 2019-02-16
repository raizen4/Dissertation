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
        public Task<HttpResponseMessage> LoginUser(LoginRequest request)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<HttpResponseMessage> CreateUser(RegisterRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
