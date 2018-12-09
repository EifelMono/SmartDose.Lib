namespace SmartDose.WcfMasterData10000
{
    public partial class ServiceResult : object
    {
        public T CastByClone<T>() where T : ServiceResult, new()
          => new T
          {
              StatusAsInt = StatusAsInt,
              Status = Status,
              Message = Message,
              Exception = Exception,
              Debug = Debug,
              Data = Data,
          };

        public object Data { get; set; }

        public bool IsOk => Status == ServiceResultStatus.Ok;
    }

    public class ServiceResult<T> : ServiceResult
    {
        public ServiceResult() : base()
        {

        }
        public new  T Data { get => (T)base.Data; set => base.Data = value; }
    }
}
