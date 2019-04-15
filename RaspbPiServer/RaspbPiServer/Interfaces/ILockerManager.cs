using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaspbPiServer.Interfaces
{
    using Enums;

    public interface ILockerManager
    {

        bool Open();

        bool Close();

        PowerTypeEnum? GetCurrentPowerType();
    }
}
