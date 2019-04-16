namespace XamarinLockerApp.ServiceModels
{
    using Enums;

    public class LockerMessage
    {
        public LockerActionEnum Action { get; set; }
        public LockerActionEnum ActionResult { get; set; }

        public string SenderDeviceId { get; set; }
        public bool HasBeenSuccessful { get; set; }
      
        public string TargetedDeviceId { get; set; }
        public string IotHubEndpoint { get; set; }

      
    }
}
