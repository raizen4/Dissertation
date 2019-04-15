using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockerApp.Helpers
{
    using Windows.Devices.Gpio;
    using Windows.UI.Popups;
    using Interfaces;
    using MvvmDialogs;

    class PiGpioController:IGpioController
    {
        private GpioController _controller;
        private GpioPin _lockPin;
        private GpioPin _pinForBackupBattery;
        private IFacade _facade;
        private readonly IDialogService _dialogService;
        private bool _hasInitialized;
        public PiGpioController(IFacade facade, IDialogService dialogService)
        {
            this._dialogService = dialogService;
            this._facade = facade;
            this._hasInitialized = false;
            Initialize();
        }
        /// <inheritdoc />
        public void Initialize()
        {

            try
            {
                this._controller = GpioController.GetDefault();
                if (this._controller == null)
                {
                    //device not supporting the controller
                    return;
                }
             
                    try
                    {
                        this._lockPin = this._controller.OpenPin(12);
                        this._pinForBackupBattery = this._controller.OpenPin(16);
                        // Latch HIGH value first. This ensures a default value when the pin is set as output
                        this._lockPin.Write(GpioPinValue.High);
                        this._lockPin.SetDriveMode(GpioPinDriveMode.Output);

                        // Latch HIGH value first. This ensures a default value when the pin is set as output
                        this._pinForBackupBattery.Write(GpioPinValue.Low);
                        this._pinForBackupBattery.SetDriveMode(GpioPinDriveMode.Input);
                        this._pinForBackupBattery.ValueChanged += PinForBackupBattery_ValueChanged;
                        this._hasInitialized = true;
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

        private async void PinForBackupBattery_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {

            if (args.Edge == GpioPinEdge.RisingEdge)
            {
                sender.Write(GpioPinValue.High);
                try
                {
                    var facadeResult = await this._facade.SendBackupBatteryNotification();
                    if (!facadeResult.HasBeenSuccessful)
                    {
                        await this._dialogService.ShowMessageDialogAsync("Power cut notification did not work.", "Warning", new[]
                        {
                            new UICommand { Label = "OK" },

                        });

                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else if(sender.Read()==GpioPinValue.High && args.Edge==GpioPinEdge.FallingEdge)
            {
                sender.Write(GpioPinValue.Low);
            }
        }

        /// <inheritdoc />
        public void CloseLocker()
        {
            if (!this._hasInitialized)
            {
                Initialize();
            }
            try
            {
                this._lockPin.Write(GpioPinValue.High);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
          
        }

        /// <inheritdoc />
        public void OpenLocker()
        {

            if (!this._hasInitialized)
            {
                Initialize();
            }
            try
            {
                this._lockPin.Write(GpioPinValue.Low);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
    }
}
