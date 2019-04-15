using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaspbPiServer.Managers
{
    using Enums;
    using Interfaces;
    using LockerApp.Interfaces;

    public class LockerManager:ILockerManager
    {
        private IGpioControlleMethods controller;

        public LockerManager(IGpioControlleMethods GpioController)
        {
            this.controller = GpioController;
        }
        /// <inheritdoc />
        public bool Open()
        {
            try
            {

                var result = this.controller.OpenLocker();
                if (result)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        /// <inheritdoc />
        public bool Close()
        {
            try
            {
                var result = this.controller.CloseLocker();
                if(result)
                 return true;
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        /// <inheritdoc />
        public PowerTypeEnum? GetCurrentPowerType()
        {
            try
            {
                var result = this.controller.GetCurrentPowerSourceStatus();
                if (result == null)
                {
                    return null;
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
