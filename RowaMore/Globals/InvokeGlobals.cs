using System;
using System.Threading.Tasks;
using RowaMore.Extensions;

namespace RowaMore.Globals
{
    public static partial class InvokeGlobals
    {
        #region CatchInvoke
        public static void CatchInvoke(Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch { }
        }

        public static TResult CatchInvoke<TResult>(Func<TResult> func)
        {
            var data = default(TResult);
            try
            {
                if (func != null)
                    return func.Invoke();
                return data;
            }
            catch
            {
                return data;
            };
        }

        public async static Task CatchInvokeAsync(Func<Task> funcAsync)
        {
            try
            {
                if (funcAsync != null)
                    await (funcAsync.Invoke()).ConfigureAwait(false);
            }
            catch { };
        }

        public async static Task<TResult> CatchInvokeAsync<TResult>(Func<Task<TResult>> funcAsync)
        {
            var data = default(TResult);
            try
            {
                if (funcAsync != null)
                    return await funcAsync.Invoke().ConfigureAwait(false);
                return data;
            }
            catch
            {
                return data;
            };
        }
        #endregion

        #region SafeInvoke
        public static (bool Ok, Exception Exception) SafeInvoke(Action action)
        {
            try
            {
                if (action == null)
                    return (false, new NullReferenceException("action is null"));
                action?.Invoke();
                return (true, null);

            }
            catch (Exception ex)
            {
                return (false, ex);
            };
        }

        public static (bool Ok, TResult Data, Exception Exception) SafeInvoke<TResult>(Func<TResult> func)
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

        public async static Task<(bool Ok, Exception Exception)> SafeInvokeAsync(Func<Task> funcAsync)
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

        public async static Task<(bool Ok, TResult Data, Exception Exception)> SafeInvokeAsync<TResult>(Func<Task<TResult>> funcAsync)
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
        #endregion
    }
}
