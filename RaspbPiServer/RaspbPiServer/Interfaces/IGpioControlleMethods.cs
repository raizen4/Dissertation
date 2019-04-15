using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockerApp.Interfaces
{
    using RaspbPiServer.Enums;

    public interface IGpioControlleMethods
    {


        bool CloseLocker();
        bool OpenLocker();

        PowerTypeEnum? GetCurrentPowerSourceStatus();
    }
}
