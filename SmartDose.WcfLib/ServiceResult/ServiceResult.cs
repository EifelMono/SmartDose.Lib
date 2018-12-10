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

        public bool IsOk => Status == ServiceResultStatus.Ok;

        public bool IsOkPlus => Status >= ServiceResultStatus.Ok;

        public bool IsError => Status < ServiceResultStatus.Ok;
    }

    public class ServiceResult<TData> : ServiceResult, IServiceResult<TData>
    {
        public ServiceResult()
        {
            Data = default;
        }

        public new TData Data { get => (TData)base.Data; set => base.Data = value; }
    }
}
