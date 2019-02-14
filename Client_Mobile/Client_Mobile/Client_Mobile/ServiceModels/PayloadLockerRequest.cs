using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.ServiceModels
{
    using Enums;

    class PayloadLockerRequest<T>:GenericLockerRequest
    {
       
        public T Payload { get; set; }
    }
}
