using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockerApp.Helpers
{
    using System.Device.Gpio;
    using Interfaces;
    using RaspbPiServer.Enums;


    class PiGpioController : IGpioControlleMethods
    {
        private IGpioController _controller;
        private bool _hasInitialized;
        private int electricLockPin = 27;
        private int pinForBackupBatteryPin = 22;

        public PiGpioController(IGpioController gpioController)
        {
            if (this._controller == null)
            {

           
            try
            {

                Console.WriteLine("In controller - instantiating GpioController instance");
              

                try
                {
                    Console.WriteLine("In controller - Begin get the controller");
                    this._controller = gpioController;
                    if (this._controller == null)
                    {
                        Console.WriteLine("In controller - instantiating GpioController instance-Gpio controller couldn't pe initialized");
                        return;
                    }
                    Console.WriteLine("In controller - Finished getting the controller");

                    Console.WriteLine("In controller - begin Initializing pins");

                    Console.WriteLine("In controller - begin Initializing electronic lock");
                    this._controller.OpenPin(this.electricLockPin, PinMode.Output);
                    this._controller.Write(this.electricLockPin, PinValue.Low);
                    Console.WriteLine("In controller - finished Initializing electronic lock");


                    Console.WriteLine("In controller - begin Initializing battery notification pin");
                    this._controller.OpenPin(this.pinForBackupBatteryPin, PinMode.Input);
                    Console.WriteLine("In controller - finished Initializing  battery notification pin");


                    Console.WriteLine("In controller - finished Initializing pins");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Something went wrong with pin initialization");
                }


            }
            catch (Exception)
            {
                Console.WriteLine("Exception when setting up gpio controller");
            }
            }
        }
     


    /// <inheritdoc />
       
     
        /// <inheritdoc />
        public bool CloseLocker()
        {
            try
            {
                this._controller.Write(this.electricLockPin,PinValue.Low);
                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
          
        }

        /// <inheritdoc />
        public bool OpenLocker()
        {

         
          
            try
            {
                this._controller.Write(this.electricLockPin, PinValue.High);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            
        }

        public PowerTypeEnum? GetCurrentPowerSourceStatus()
        {
           
            try
            {
                var currentStatus = this._controller.Read(this.pinForBackupBatteryPin);
                if (currentStatus == PinValue.High)
                {
                    return PowerTypeEnum.BackupPower;
                }
                else
                {
                    return PowerTypeEnum.MainPower;
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

    }
}
