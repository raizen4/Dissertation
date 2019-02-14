using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.ServiceModels
{
    class ResponseData<T>:ResponseBase
    {
        public T Content { get; set; }

    }
}
