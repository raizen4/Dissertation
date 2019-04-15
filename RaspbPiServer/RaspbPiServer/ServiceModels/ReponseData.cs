using System;
using System.Collections.Generic;
using System.Text;

namespace LockerApp.ServiceModels
{
    public class ResponseData<T>:ResponseBase
    {
        public T Content { get; set; }

    }
}
