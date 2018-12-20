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

        #region IServiceResult ServiceResultInvoke 
        public static TResult ServiceResultInvoke<TResult>(this Func<TResult> func) where TResult : IServiceResult, new()
        {
            try
            {
                if (func == null)
                    return new TResult { Status = ServiceResultStatus.ErrorInvalidateArgs };
                return func.Invoke();
            }
            catch (Exception ex)
            {
                return new TResult
                {
                    Status = ServiceResultStatus.Exception,
                    Exception = ex,
                };
            }
        }

        public static TResult ServiceResultInvoke<T1, TResult>(this Func<T1, TResult> func, T1 argT1) where TResult : IServiceResult, new()
        {
            try
            {
                if (func == null)
                    return new TResult { Status = ServiceResultStatus.ErrorInvalidateArgs };
                return func.Invoke(argT1);
            }
            catch (Exception ex)
            {
                return new TResult
                {
                    Status = ServiceResultStatus.Exception,
                    Exception = ex,
                };
            }
        }

        public static TResult ServiceResultInvoke<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 argT1, T2 argT2) where TResult : ServiceResult, new()
        {
            try
            {
                if (func == null)
                    return new TResult { Status = ServiceResultStatus.ErrorInvalidateArgs };
                return func.Invoke(argT1, argT2);
            }
            catch (Exception ex)
            {
                return new TResult
                {
                    Status = ServiceResultStatus.Exception,
                    Exception = ex,
                };
            }
        }

        public static TResult ServiceResultInvoke<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T1 argT1, T2 argT2, T3 argT3) where TResult : ServiceResult, new()
        {
            try
            {
                if (func == null)
                    return new TResult { Status = ServiceResultStatus.ErrorInvalidateArgs };
                return func.Invoke(argT1, argT2, argT3);
            }
            catch (Exception ex)
            {
                return new TResult
                {
                    Status = ServiceResultStatus.Exception,
                    Exception = ex,
                };
            }
        }

        public static TResult ServiceResultInvoke<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func, T1 argT1, T2 argT2, T3 argT3, T4 argT4) where TResult : ServiceResult, new()
        {
            try
            {
                if (func == null)
                    return new TResult { Status = ServiceResultStatus.ErrorInvalidateArgs };
                return func.Invoke(argT1, argT2, argT3, argT4);
            }
            catch (Exception ex)
            {
                return new TResult
                {
                    Status = ServiceResultStatus.Exception,
                    Exception = ex,
                };
            }
        }
        public static TResult ServiceResultInvoke<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func, T1 argT1, T2 argT2, T3 argT3, T4 argT4, T5 argT5) where TResult : ServiceResult, new()
        {
            try
            {
                if (func == null)
                    return new TResult { Status = ServiceResultStatus.ErrorInvalidateArgs };
                return func.Invoke(argT1, argT2, argT3, argT4, argT5);
            }
            catch (Exception ex)
            {
                return new TResult
                {
                    Status = ServiceResultStatus.Exception,
                    Exception = ex,
                };
            }
        }
        #endregion

        #region <Task<IServiceResult>> ServiceResultInvokeAsync
        public async static Task<TResult> ServiceResultInvokeAsync<TResult>(this Func<Task<TResult>> funcAsync) where TResult : IServiceResult, new()
        {
            try
            {
                if (funcAsync == null)
                    return new TResult { Status = ServiceResultStatus.ErrorInvalidateArgs };
                return await funcAsync.Invoke().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new TResult
                {
                    Status = ServiceResultStatus.Exception,
                    Exception = ex,
                };
            }
        }

        public async static Task<TResult> ServiceResultInvokeAsync<T1, TResult>(this Func<T1, Task<TResult>> funcAsync, T1 argT1) where TResult : IServiceResult, new()
        {
            try
            {
                if (funcAsync == null)
                    return new TResult { Status = ServiceResultStatus.ErrorInvalidateArgs };
                return await funcAsync.Invoke(argT1).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new TResult
                {
                    Status = ServiceResultStatus.Exception,
                    Exception = ex,
                };
            }
        }

        public async static Task<TResult> ServiceResultInvokeAsync<T1, T2, TResult>(this Func<T1, T2, Task<TResult>> funcAsync, T1 argT1, T2 argT2) where TResult : IServiceResult, new()
        {
            try
            {
                if (funcAsync == null)
                    return new TResult { Status = ServiceResultStatus.ErrorInvalidateArgs };
                return await funcAsync.Invoke(argT1, argT2).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new TResult
                {
                    Status = ServiceResultStatus.Exception,
                    Exception = ex,
                };
            }
        }

        public async static Task<TResult> ServiceResultInvokeAsync<T1, T2, T3, TResult>(this Func<T1, T2, T3, Task<TResult>> funcAsync, T1 argT1, T2 argT2, T3 argT3) where TResult : IServiceResult, new()
        {
            try
            {
                if (funcAsync == null)
                    return new TResult { Status = ServiceResultStatus.ErrorInvalidateArgs };
                return await funcAsync.Invoke(argT1, argT2, argT3).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new TResult
                {
                    Status = ServiceResultStatus.Exception,
                    Exception = ex,
                };
            }
        }

        public async static Task<TResult> ServiceResultInvokeAsync<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, Task<TResult>> funcAsync, T1 argT1, T2 argT2, T3 argT3, T4 argT4) where TResult : IServiceResult, new()
        {
            try
            {
                if (funcAsync == null)
                    return new TResult { Status = ServiceResultStatus.ErrorInvalidateArgs };
                return await funcAsync.Invoke(argT1, argT2, argT3, argT4).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new TResult
                {
                    Status = ServiceResultStatus.Exception,
                    Exception = ex,
                };
            }
        }

        public async static Task<TResult> ServiceResultInvokeAsync<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, Task<TResult>> funcAsync, T1 argT1, T2 argT2, T3 argT3, T4 argT4, T5 argT5) where TResult : IServiceResult, new()
        {
            try
            {
                if (funcAsync == null)
                    return new TResult { Status = ServiceResultStatus.ErrorInvalidateArgs };
                return await funcAsync.Invoke(argT1, argT2, argT3, argT4, argT5).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new TResult
                {
                    Status = ServiceResultStatus.Exception,
                    Exception = ex,
                };
            }
        }
        #endregion

    }
}
