namespace XamarinLockerApp.ServiceModels
{
    using Enums;
    using Models;

    class ActionRequest
    {
        public LockerActionEnum Action { get; set; }
        public Pin Pin { get; set; } 


    }
}
