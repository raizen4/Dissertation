using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.ServiceModels
{
    public class ResponseBase
    {
        public bool IsSuccessful { get; set; }
        public string Error { get; set; }
    }
}
