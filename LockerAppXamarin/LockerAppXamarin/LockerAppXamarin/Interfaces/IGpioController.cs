using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockerAppXamarin.Interfaces
{
    public interface IGpioController
    {

        void Initialize();

        void CloseLocker();
        void OpenLocker();
    }
}
