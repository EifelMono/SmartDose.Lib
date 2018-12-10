using System;

namespace SmartDose.WcfLib
{

   

    public class ServiceResult : IServiceResult
    {
        public int Status { get; set; }

        public string StatusAsString { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public string Debug { get; set; }

        public object Data { get; set; }

        public bool IsOk => Status == 0;

        public bool IsOkBut => (int)Status >= (int)ServiceResultStatus.Ok;

        public bool IsError => (int)Status < (int)ServiceResultStatus.Ok;

        public T CastByClone<T>() where T : ServiceResult, new()
            => new T
            {
                Status = Status,
                StatusAsString = StatusAsString,
                Message = Message,
                Exception = Exception,
                Debug = Debug,
                Data = Data,
            };
    }

    public class ServiceResult<T> : ServiceResult, IServiceResult<T>
    {
        public ServiceResult()
        {
            Data = default;
        }

        public new T Data { get => (T)base.Data; set => base.Data = value; }
    }
}
