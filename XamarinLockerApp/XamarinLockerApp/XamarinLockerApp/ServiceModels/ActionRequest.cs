namespace XamarinLockerApp.ServiceModels
{
    using Enums;
    using Models;

    class ActionRequest
    {
        public LockerActionEnum ActionType { get; set; }
        public Pin Pin { get; set; } 


    }
}
