using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDose.WcfLib
{
    public interface IServiceResult
    {
        int StatusAsInt { get; set; }
        // ServiceResultStatus Status { get; set; }
        string Message { get; set; }

        Exception Exception { get; set; }

        string Debug { get; set; }

        object Data { get; set; }
    }

    public interface IServiceResult<T>: IServiceResult
    {
        new T Data { get; set; }
    }
}
