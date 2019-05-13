namespace XamarinLockerApp.ServiceModels
{
    public class ResponseData<T>:ResponseBase
    {
        public T Content { get; set; }

    }
}
