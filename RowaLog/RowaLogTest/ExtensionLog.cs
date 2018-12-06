using Rowa.Lib.Log.Extensions;
using System;

namespace RowaLogTest
{
    class ExtensionLog
    {
        public void Log()
        {
            this.Debug("Extension - MyDebug {0}", 1);
            this.Debug(123, "Extension - MyDebug TaskId {0}", 2);
            this.Debug(555, 123, "Extension - MyDebug AllIds {0}", 3);

            this.Info("Extension - MyInfo {0}", 1);
            this.Info(123, "Extension - MyInfo TaskId {0}", 2);
            this.Info(555, 123, "Extension - MyInfo AllIds {0}", 3);

            this.Warning("Extension - MyWarning {0}", 1);
            this.Warning(123, "Extension - MyWarning TaskId {0}", 2);
            this.Warning(555, 123, "Extension - MyWarning AllIds {0}", 3);

            this.UserIn("Extension - MyUserIn {0}", 1);
            this.UserIn(123, "Extension - MyUserIn TaskId {0}", 2);
            this.UserIn(555, 123, "Extension - MyUserIn AllIds {0}", 3);

            this.ExtIf("Extension - MyExtIf {0}", 1);
            this.ExtIf(123, "Extension - MyExtIf TaskId {0}", 2);
            this.ExtIf(555, 123, "Extension - MyExtIf AllIds {0}", 3);

            var ex = new ApplicationException("An Error!", new ApplicationException("An inner Error!"));

            this.Error("Extension - MyError {0}", 1);
            this.Error(123, "Extension - MyError TaskId {0}", 2);
            this.Error(555, 123, "Extension - MyError AllIds {0}", 3);
            this.Error(ex, "Extension - MyError", 4);
            this.Error(ex, 123, "Extension - MyError with ex and task id {0}", 5);
            this.Error(ex, 555, 123, "Extension - MyError with ex and all ids {0}", 6);

            this.Fatal("Extension - MyFatal {0}", 1);
            this.Fatal(123, "Extension - MyFatal TaskId {0}", 2);
            this.Fatal(555, 123, "Extension - MyFatal AllIds {0}", 3);
            this.Fatal(ex, "Extension - MyFatal with ex {0}", 4);
            this.Fatal(ex, 123, "Extension - MyFatal with ex and task id {0}", 5);
            this.Fatal(ex, 555, 123, "Extension - MyFatal with ex and all ids {0}", 6);

            this.Audit("Extension - MyAudit {0}", 1);
            this.Audit(ex, "Extension - MyAudit", 4);
            this.Audit("Extension - MyAudit {0}", 1);
            this.Audit(123, "Extension - MyAudit TaskId {0}", 2);
            this.Audit(555, 123, "Extension - MyAudit AllIds {0}", 3);
            this.Audit(ex, "Extension - MyAudit with ex {0}", 4);
            this.Audit(ex, 123, "Extension - MyAudit with ex and task id {0}", 5);
            this.Audit(ex, 555, 123, "Extension - MyAudit with ex and all ids {0}", 6);
        }
    }
}
