using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rowa.Lib.Log.Observer
{
    internal static class LogSubjectExtensions
    {
        public static void ExecuteLoggerError(this LogSubject subject,  LogErrorEventArgs args)
        { 
            if (subject == null) return;
            if (args == null) return;

            subject.Notify(new Notification(LogEventType.Error, args));
        }
    }
}
