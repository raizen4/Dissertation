using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockerApp.Models
{
    public class LockerInfo
    {
        public string IotHubConnectionString { get; set; }
        public string DeviceId { get; set; }
     
        public string Token { get; set; }
    }
}
