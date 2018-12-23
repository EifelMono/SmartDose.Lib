using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartDose.WcfLib
{
    public static class ServiceResultExtensions
    {
        public static bool IsOk(this IServiceResult thisValue)
            => thisValue == null ? false : thisValue.Status == 0;
        public static bool IsOkPlus(this IServiceResult thisValue)
            => thisValue == null ? false : thisValue.Status >= 0;
        public static bool IsError(this IServiceResult thisValue)
            => thisValue == null ? false : thisValue.Status < 0;

        public static string ToErrorString(this IServiceResult thisValue)
            => $"Status= {thisValue.Status}[{thisValue.StatusAsString}]";

        public static T CastByClone<T>(this IServiceResult thisValue, bool withData = true) where T : IServiceResult, new()
           => new T
           {
               Status = thisValue.Status,
               StatusAsString = thisValue.StatusAsString,
               Message = thisValue.Message,
               Exception = thisValue.Exception,
               Debug = thisValue.Debug,
               Data = withData ? thisValue.Data : default,
           };
    }
}
