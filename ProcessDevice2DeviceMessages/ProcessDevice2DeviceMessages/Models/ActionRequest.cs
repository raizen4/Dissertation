using Client_Mobile.Enums;

namespace Client_Mobile.Interfaces
{
    public class ActionRequest
    {
        public string Token { get; internal set; }
        public LockerActionEnum Action { get; internal set; }
    }
}