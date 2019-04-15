using System;
using System.Collections.Generic;
using System.Text;

namespace LockerAppXamarin.ServiceModels
{
    using Enums;

    public class ResponseData<T>:ResponseBase
    {
        public T Content { get; set; }

    }
}
