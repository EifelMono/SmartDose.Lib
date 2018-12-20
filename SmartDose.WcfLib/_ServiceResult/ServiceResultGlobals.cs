using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartDose.WcfLib
{
    public static class ServiceResultGlobals
    {
        #region IServiceResult ServiceResultInvoke 
        public static TResult ServiceResultInvoke<TResult>(Func<TResult> func) where TResult : IServiceResult, new()
            => func.ServiceResultInvoke();

        public static TResult ServiceResultInvoke<T1, TResult>(Func<T1, TResult> func, T1 argT1) where TResult : IServiceResult, new()
            => func.ServiceResultInvoke(argT1);

        public static TResult ServiceResultInvoke<T1, T2, TResult>(Func<T1, T2, TResult> func, T1 argT1, T2 argT2) where TResult : ServiceResult, new()
            => func.ServiceResultInvoke(argT1, argT2);

        public static TResult ServiceResultInvoke<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, T1 argT1, T2 argT2, T3 argT3) where TResult : ServiceResult, new()
            => func.ServiceResultInvoke(argT1, argT2, argT3);

        public static TResult ServiceResultInvoke<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, T1 argT1, T2 argT2, T3 argT3, T4 argT4) where TResult : ServiceResult, new()
            => func.ServiceResultInvoke(argT1, argT2, argT3, argT4);
        public static TResult ServiceResultInvoke<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func, T1 argT1, T2 argT2, T3 argT3, T4 argT4, T5 argT5) where TResult : ServiceResult, new()
            => func.ServiceResultInvoke(argT1, argT2, argT3, argT4, argT5);
        #endregion

        #region <Task<IServiceResult>> ServiceResultInvokeAsync
        public async static Task<TResult> ServiceResultInvokeAsync<TResult>(Func<Task<TResult>> funcAsync) where TResult : IServiceResult, new()
            => await funcAsync.ServiceResultInvokeAsync().ConfigureAwait(false);
        public async static Task<TResult> ServiceResultInvokeAsync<T1, TResult>(Func<T1, Task<TResult>> funcAsync, T1 argT1) where TResult : IServiceResult, new()
            => await funcAsync.ServiceResultInvokeAsync(argT1).ConfigureAwait(false);

        public async static Task<TResult> ServiceResultInvokeAsync<T1, T2, TResult>(Func<T1, T2, Task<TResult>> funcAsync, T1 argT1, T2 argT2) where TResult : IServiceResult, new()
            => await funcAsync.ServiceResultInvokeAsync(argT1, argT2).ConfigureAwait(false);

        public async static Task<TResult> ServiceResultInvokeAsync<T1, T2, T3, TResult>(Func<T1, T2, T3, Task<TResult>> funcAsync, T1 argT1, T2 argT2, T3 argT3) where TResult : IServiceResult, new()
            => await funcAsync.ServiceResultInvokeAsync(argT1, argT2, argT3).ConfigureAwait(false);

        public async static Task<TResult> ServiceResultInvokeAsync<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, Task<TResult>> funcAsync, T1 argT1, T2 argT2, T3 argT3, T4 argT4) where TResult : IServiceResult, new()
            => await funcAsync.ServiceResultInvokeAsync(argT1, argT2, argT3, argT4).ConfigureAwait(false);

        public async static Task<TResult> ServiceResultInvokeAsync<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, Task<TResult>> funcAsync, T1 argT1, T2 argT2, T3 argT3, T4 argT4, T5 argT5) where TResult : IServiceResult, new()
            => await funcAsync.ServiceResultInvokeAsync(argT1, argT2, argT3, argT4, argT5).ConfigureAwait(false);
        #endregion
    }
}
