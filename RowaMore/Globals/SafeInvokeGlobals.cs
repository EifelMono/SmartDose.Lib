using System;
using System.Threading.Tasks;
using RowaMore.Extensions;

namespace RowaMore.Globals
{
    public static class SafeInvokeGlobals
    {
        #region Action
        public static (bool Ok, Exception Exception) SafeInvoke(Action action)
            => action.SafeInvoke();

        public static (bool Ok, Exception Exception) SafeInvoke<T1>(Action<T1> action, T1 argT1)
            => action.SafeInvoke(argT1);

        public static (bool Ok, Exception Exception) SafeInvoke<T1, T2>(Action<T1, T2> action, T1 argT1, T2 argT2)
            => action.SafeInvoke(argT1, argT2);

        public static (bool Ok, Exception Exception) SafeInvoke<T1, T2, T3>(Action<T1, T2, T3> action, T1 argT1, T2 argT2, T3 argT3)
            => action.SafeInvoke(argT1, argT2, argT3);

        public static (bool Ok, Exception Exception) SafeInvoke<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 argT1, T2 argT2, T3 argT3, T4 argT4)
            => action.SafeInvoke(argT1, argT2, argT3, argT4);

        public static (bool Ok, Exception Exception) SafeInvoke<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action, T1 argT1, T2 argT2, T3 argT3, T4 argT4, T5 argT5)
            => action.SafeInvoke(argT1, argT2, argT3, argT4, argT5);
        #endregion

        #region Func<TResult>
        public static (bool Ok, TResult Data, Exception Exception) SafeInvoke<TResult>(Func<TResult> func)
            => func.SafeInvoke();

        public static (bool Ok, TResult Data, Exception Exception) SafeInvoke<T1, TResult>(Func<T1, TResult> func, T1 argT1)
            => func.SafeInvoke(argT1);

        public static (bool Ok, TResult Data, Exception Exception) SafeInvoke<T1, T2, TResult>(Func<T1, T2, TResult> func, T1 argT1, T2 argT2)
            => func.SafeInvoke(argT1, argT2);

        public static (bool Ok, TResult Data, Exception Exception) SafeInvoke<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, T1 argT1, T2 argT2, T3 argT3)
            => func.SafeInvoke(argT1, argT2, argT3);
        public static (bool Ok, TResult Data, Exception Exception) SafeInvoke<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, T1 argT1, T2 argT2, T3 argT3, T4 argT4)
            => func.SafeInvoke(argT1, argT2, argT3, argT4);

        public static (bool Ok, TResult Data, Exception Exception) SafeInvoke<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func, T1 argT1, T2 argT2, T3 argT3, T4 argT4, T5 argT5)
            => func.SafeInvoke(argT1, argT2, argT3, argT4, argT5);
        #endregion

        #region FuncAsync<Task>
        public async static Task<(bool Ok, Exception Exception)> SafeInvokeAsync(Func<Task> funcAsync)
            => await funcAsync.SafeInvokeAsync().ConfigureAwait(false);

        public async static Task<(bool Ok, Exception Exception)> SafeInvokeAsync<T1>(Func<T1, Task> funcAsync, T1 argT1)
             => await funcAsync.SafeInvokeAsync(argT1).ConfigureAwait(false);

        public async static Task<(bool Ok, Exception Exception)> SafeInvokeAsync<T1, T2>(Func<T1, T2, Task> funcAsync, T1 argT1, T2 argT2)
             => await funcAsync.SafeInvokeAsync(argT1, argT2).ConfigureAwait(false);
        #endregion

        #region FuncAsync<Task<TResult>>
        public async static Task<(bool Ok, TResult Data, Exception Exception)> SafeInvokeAsync<TResult>(Func<Task<TResult>> funcAsync)
            => await funcAsync.SafeInvokeAsync().ConfigureAwait(false);

        public async static Task<(bool Ok, TResult Data, Exception Exception)> SafeInvokeAsync<TResult, T1>(Func<T1, Task<TResult>> funcAsync, T1 argT1)
            => await funcAsync.SafeInvokeAsync(argT1).ConfigureAwait(false);

        public async static Task<(bool Ok, TResult Data, Exception Exception)> SafeInvokeAsync<TResult, T1, T2>(Func<T1, T2, Task<TResult>> funcAsync, T1 argT1, T2 argT2)
            => await funcAsync.SafeInvokeAsync(argT1, argT2).ConfigureAwait(false);

        #endregion
    }
}
