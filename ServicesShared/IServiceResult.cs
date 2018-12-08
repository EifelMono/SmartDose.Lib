using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesShared
{
    public enum ServiceResultStatus
    {
        #region Ok, OK
        [EnumMember]
        Ok = 0,
        #endregion

        #region Ok But
        [EnumMember]
        OkButDataIsNull = 10,
        [EnumMember]
        OkButItemNotFound = 11,
        [EnumMember]
        OkButListIsEmpty = 12,
        #endregion

        #region Error
        [EnumMember]
        Error = -1,
        [EnumMember]
        ErrorException = -2,
        [EnumMember]
        ErrorConnection = -3,
        [EnumMember]
        ErrorNoEnumForInt = -4,
        [EnumMember]
        ErrorInvalidateArgs = -10,
        [EnumMember]
        ErrorCouldNotDelete = -11,
        [EnumMember]
        ErrorIdentifierNotFound = -12,
        [EnumMember]
        ErrorNothingFound = -13,
        [EnumMember]
        ErrorFailed = -14,
        #endregion
    }

    public abstract class ServiceResult
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
            if (typeof(T).IsGenericType)
                TypeName = typeof(T).GetGenericTypeDefinition().Name;
            else
                TypeName = typeof(T).Name;
        }

        [DataMember]
        public new T Data { get => (T)base.Data; set => base.Data = value; }
    }
}
