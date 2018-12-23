using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartDose.WcfLib
{
    public static partial class InvokeGlobals
    {
        #region IServiceResult ServiceResultInvoke 
        public static TResult ServiceResultInvoke<TResult>(Func<TResult> func) where TResult : IServiceResult, new()
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
        #endregion

        #region <Task<IServiceResult>> ServiceResultInvokeAsync
        public async static Task<TResult> ServiceResultInvokeAsync<TResult>(Func<Task<TResult>> funcAsync) where TResult : IServiceResult, new()
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
        #endregion
    }
}
