﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LockerApp.ServiceModels
{
    public class ResponseBase
    {
        public bool HasBeenSuccessful { get; set; }
        public string Error { get; set; }
    }
}
