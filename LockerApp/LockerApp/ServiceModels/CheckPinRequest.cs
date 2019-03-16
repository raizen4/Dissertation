using LockerApp.Models;

namespace LockerApp.Interfaces
{
    public class CheckPinRequest
    {
        public Pin PinToBeChecked { get; set; }
    }
}