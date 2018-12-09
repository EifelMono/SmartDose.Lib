using System;

namespace SmartDose.WcfLib
{
    public class ServiceResult
    {
        public int StatusAsInt { get; set; }

        public ServiceResultStatus Status
        {
            get =>
                Enum.IsDefined(typeof(ServiceResultStatus), StatusAsInt)
                    ? (ServiceResultStatus)StatusAsInt
                    : ServiceResultStatus.ErrorNoEnumForInt;
            set => StatusAsInt = (int)value;
        }
        public string Message { get; set; }

        public Exception Exception { get; set; }

        public string Debug { get; set; }

        public object Data { get; set; }

        public bool IsOk => Status == ServiceResultStatus.Ok;

        public bool IsOkBut => (int)Status >= (int)ServiceResultStatus.Ok;

        public bool IsError => (int)Status < (int)ServiceResultStatus.Ok;

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
    }

    public class ServiceResult<T> : ServiceResult
    {
        public ServiceResult()
        {
            Data = default(T);
        }

        public new T Data { get => (T)base.Data; set => base.Data = value; }
    }
}
