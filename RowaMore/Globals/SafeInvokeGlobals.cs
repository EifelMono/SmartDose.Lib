using System;
using System.Collections.Generic;
using System.Text;
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
        public static (bool Ok, TResult Data, Exception Exception) SafeInvoke<TResult>(this Func<TResult> func)
        {
            var data = default(TResult);
            try
            {
                if (func == null)
                    return (false, data, new NullReferenceException("func is null"));
                return (true, func.Invoke(), null);

            }
            catch (Exception ex)
            {
                return (false, data, ex);
            };
        }

        public static (bool Ok, TResult Data, Exception Exception) SafeInvoke<T1, TResult>(this Func<T1, TResult> func, T1 argT1)
        {
            var data = default(TResult);
            try
            {
                if (func == null)
                    return (false, data, new NullReferenceException("func is null"));
                return (true, func.Invoke(argT1), null);
            }
            catch (Exception ex)
            {
                return (false, data, ex);
            };
        }

        public static (bool Ok, TResult Data, Exception Exception) SafeInvoke<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 argT1, T2 argT2)
        {
            var data = default(TResult);
            try
            {
                if (func == null)
                    return (false, data, new NullReferenceException("func is null"));
                return (true, func.Invoke(argT1, argT2), null);
            }
            catch (Exception ex)
            {
                return (false, data, ex);
            };
        }

        public static (bool Ok, TResult Data, Exception Exception) SafeInvoke<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T1 argT1, T2 argT2, T3 argT3)
        {
            var data = default(TResult);
            try
            {
                if (func == null)
                    return (false, data, new NullReferenceException("func is null"));
                return (true, func.Invoke(argT1, argT2, argT3), null);
            }
            catch (Exception ex)
            {
                return (false, data, ex);
            };
        }

        public static (bool Ok, TResult Data, Exception Exception) SafeInvoke<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func, T1 argT1, T2 argT2, T3 argT3, T4 argT4)
        {
            var data = default(TResult);
            try
            {
                if (func == null)
                    return (false, data, new NullReferenceException("func is null"));
                return (true, func.Invoke(argT1, argT2, argT3, argT4), null);
            }
            catch (Exception ex)
            {
                return (false, data, ex);
            };
        }

        public static (bool Ok, TResult Data, Exception Exception) SafeInvoke<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func, T1 argT1, T2 argT2, T3 argT3, T4 argT4, T5 argT5)
        {
            var data = default(TResult);
            try
            {
                if (func == null)
                    return (false, data, new NullReferenceException("func is null"));
                return (true, func.Invoke(argT1, argT2, argT3, argT4, argT5), null);
            }
            catch (Exception ex)
            {
                return (false, data, ex);
            };
        }
        #endregion

        #region FuncAsync<Task>
        public async static Task<(bool Ok, Exception Exception)> SafeInvokeAsync(this Func<Task> funcAsync)
        {
            try
            {
                if (funcAsync == null)
                    return (false, new NullReferenceException("funcAsync is null"));
                await (funcAsync.Invoke()).ConfigureAwait(false);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex);
            };
        }

        public async static Task<(bool Ok, Exception Exception)> SafeInvokeAsync<T1>(this Func<T1, Task> funcAsync, T1 argT1)
        {
            try
            {
                if (funcAsync == null)
                    return (false, new NullReferenceException("funcAsync is null"));
                await (funcAsync.Invoke(argT1)).ConfigureAwait(false);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex);
            };
        }

        public async static Task<(bool Ok, Exception Exception)> SafeInvokeAsync<T1, T2>(this Func<T1, T2, Task> funcAsync, T1 argT1, T2 argT2)
        {
            try
            {
                if (funcAsync == null)
                    return (false, new NullReferenceException("funcAsync is null"));
                await (funcAsync.Invoke(argT1, argT2)).ConfigureAwait(false);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex);
            };
        }
        #endregion

        #region FuncAsync<Task<TResult>>
        public async static Task<(bool Ok, TResult Data, Exception Exception)> SafeInvokeAsync<TResult>(this Func<Task<TResult>> funcAsync)
        {
            var data = default(TResult);
            try
            {
                if (funcAsync == null)
                    return (false, data, new NullReferenceException("funcAsync is null"));
                return (true, await (funcAsync.Invoke()).ConfigureAwait(false), null);
            }
            catch (Exception ex)
            {
                return (false, data, ex);
            };
        }

        public async static Task<(bool Ok, TResult Data, Exception Exception)> SafeInvokeAsync<TResult, T1>(this Func<T1, Task<TResult>> funcAsync, T1 argT1)
        {
            var data = default(TResult);
            try
            {
                if (funcAsync == null)
                    return (false, data, new NullReferenceException("funcAsync is null"));
                return (true, await (funcAsync.Invoke(argT1)).ConfigureAwait(false), null);
            }
            catch (Exception ex)
            {
                return (false, data, ex);
            };
        }

        public async static Task<(bool Ok, TResult Data, Exception Exception)> SafeInvokeAsync<TResult, T1, T2>(this Func<T1, T2, Task<TResult>> funcAsync, T1 argT1, T2 argT2)
        {
            var data = default(TResult);
            try
            {
                if (funcAsync == null)
                    return (false, data, new NullReferenceException("funcAsync is null"));
                return (true, await (funcAsync.Invoke(argT1, argT2)).ConfigureAwait(false), null);
            }
            catch (Exception ex)
            {
                return (false, data, ex);
            };
        }
        #endregion
    }
}
