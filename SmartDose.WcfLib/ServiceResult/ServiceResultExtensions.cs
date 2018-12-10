using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDose.WcfLib
{
    public static class ServiceResultExtensions
    {
        public static bool IsOk(this IServiceResult thisValue)
            => thisValue == null ? false : thisValue.StatusAsInt == 0;

        public static T CastByIClone<T>(this IServiceResult thisValue) where T : ServiceResult, new()
            => new T
            {
                StatusAsInt = thisValue.StatusAsInt,
                // Status = thisValue.Status,
                Message = thisValue.Message,
                Exception = thisValue.Exception,
                Debug = thisValue.Debug,
                Data = thisValue.Data,
            };
    }
}
