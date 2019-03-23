using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockerApp.Models
{
    public class Locker
    {
        public string ConnectionString { get; set; }
        public string SymmetricKey { get; set; }
        public string DeviceId { get; set; }
    }
}
