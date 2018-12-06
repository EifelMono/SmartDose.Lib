#if MasterData10000
namespace MasterData10000
#elif MasterData9002
namespace MasterData9002
#else
namespace ConnectedService
#endif
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
        public T Data { get => (T)base.Data; set => base.Data = value; }
    }
}
