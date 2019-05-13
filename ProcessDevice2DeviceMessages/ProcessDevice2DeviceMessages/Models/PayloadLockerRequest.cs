using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.ServiceModels
{
    using Enums;

    public class PayloadLockerRequest<T>:GenericLockerRequest
    {
       
        public T Payload { get; set; }
    }
}
