﻿using SmartDose.WcfLib;

namespace SmartDose.WcfMasterData10000
{
    public partial class ServiceResult : object, IServiceResult
    {
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

        public object Data { get; set; }
    }

    public class ServiceResult<T> : ServiceResult, IServiceResult<T>
    {
        public ServiceResult() : base()
        {

        }
        public new  T Data { get => (T)base.Data; set => base.Data = value; }
    }
}
