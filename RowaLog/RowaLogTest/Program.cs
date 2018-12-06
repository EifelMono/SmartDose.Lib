using Rowa.Lib.Log;
using System;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Xml;

namespace RowaLogTest
{
    static class Program
    {
        static void Main(string[] args)
        {
            // Initialize logging with default configuration file.
            LogManager.Initialize("MyProduct", "MyComponent", true);
            LogManager.InitCleanup();
            //Logger.Setup();
            // get a logger instance
            //            var logger = LogManager.GetLogger("MyLogger");
            var logger = LogManager.GetLogger("MyLogger");

            //var duration = TimeSpan.FromSeconds(1);
            //var sw = Stopwatch.StartNew();
            //long counter = 0;
            //while (sw.Elapsed < duration)
            //{
            //    counter++;

            //    //Json String Test (with Newtonsoft.Json NuGet)
            //    //using (StreamReader file = File.OpenText(@"C:\Users\Nico.Reinemer\Downloads\Maps\Map_Player_16_4.mdto"))
            //    //using (JsonTextReader reader = new JsonTextReader(file))    --> needs newtonsoft.json
            //    //{
            //    //    JObject o2 = (JObject)JToken.ReadFrom(reader);
            //    //    logger.Audit(1,2,o2.ToString());
            //    //}


            //    //logger.Info(123, 12, "Audit Log {0}       {1}", counter, new IPEndPoint(1,2));
            //    logger.Error(new Exception("WOOHOOghjghjgjgj"), 123, 12, "Error Log {0}       {1}", counter, new IPEndPoint(1, 2));
            //    logger.Info(123, 12, "Audit Log {0}       {1}", counter, new IPEndPoint(1, 2));

            //    logger.Debug("abc");
            //    logger.Debug("{hi{s{ia}l}dl}");
            //    logger.Audit(123, 12, "Audit Log {0}", counter);
            //    logger.Warning(123, 12, "Audit Log {0}", counter);
            //    logger.Info(123, 12, "Audit Log {0}", counter);
            //    logger.UserIn(123, 12, "Audit Log {0}", counter);
            //    logger.Error(123, 12, "Audit Log {0}", counter);
            //    logger.Fatal(123, 12, "Audit Log {0}", counter);
            //}





            //    logger.Debug("MyDebug {0}", 1);


            //logger.Debug(123, "MyDebug TaskId {0}", 2);
            //logger.Debug(555, 123, "MyDebug AllIds {0}", 3);

            //logger.Info("MyInfo {0}", 1);
            //logger.Info(123, "MyInfo TaskId {0}", 2);
            //logger.Info(555, 123, "MyInfo AllIds {0}", 3);

            //logger.Warning("MyWarning {0}", 1);
            //logger.Warning(123, "MyWarning TaskId {0}", 2);
            //logger.Warning(555, 123, "MyWarning AllIds {0}", 3);

            //logger.UserIn("MyUserIn {0}", 1);
            //logger.UserIn(123, "MyUserIn TaskId {0}", 2);
            //logger.UserIn(555, 123, "MyUserIn AllIds {0}", 3);

            //logger.ExtIf("MyExtIf {0}", 1);
            //logger.ExtIf(123, "MyExtIf TaskId {0}", 2);
            //logger.ExtIf(555, 123, "MyExtIf AllIds {0}", 3);

            //var ex = new ApplicationException("An Error!", new ApplicationException("An inner Error!"));

            //logger.Error("MyError {0}", 1);
            //logger.Error(123, "MyError TaskId {0}", 2);
            //logger.Error(555, 123, "MyError AllIds {0}", 3);
            //logger.Error(ex, "MyError with ex {0}", 4);
            //logger.Error(ex, 123, "MyError with ex and task id {0}", 5);
            //logger.Error(ex, 555, 123, "MyError with ex and all ids {0}", 6);

            //logger.Fatal("MyFatal {0}", 1);
            //logger.Fatal(123, "MyFatal TaskId {0}", 2);
            //logger.Fatal(555, 123, "MyFatal AllIds {0}", 3);
            //logger.Fatal(ex, "MyFatal with ex {0}", 4);
            //logger.Fatal(ex, 123, "MyFatal with ex and task id {0}", 5);
            //logger.Fatal(ex, 555, 123, "MyFatal with ex and all ids {0}", 6);

            //string test = string.Empty;
            ////It also works with the normal logger
            //logger.Audit("MyAudit {0}", test);
            //logger.Audit(ex, "MyAudit with ex {0}", 4);

            //const string jsonString = "{\"employees\":[" +
            //                          "{ \"firstName\":\"John\", \"lastName\":\"Doe\"}," +
            //                          "{ \"firstName\":\"Anna\", \"lastName\":\"Smith\"}," +
            //                          "{ \"firstName\":\"Peter\", \"lastName\":\"Jones\"}]}";

            //logger.Audit(jsonString);
            //logger.Audit(jsonString);
            //logger.Audit(jsonString);
            //logger.Audit(jsonString);

            ////Custom Audit Logger
            //var auditLogger = LogManager.GetAuditLogger("MyAudit");

            //auditLogger.Audit("MyCustomAudit {0}", 1);
            //auditLogger.Audit(ex, "MyCustomAudit with ex {0}", 4);

            //using (var wwi = LogManager.GetWwi("Wawi", "192.168.64.1", 6050))
            //{
            //    wwi.LogMessage("<xml>an incoming message</xml>");
            //    wwi.LogMessage("<xml>an outgoing message</xml>", false);
            //}

            //  var extensionLog = new ExtensionLog();
            //  extensionLog.Log();


            try
            {
                
                throw new AbandonedMutexException("I am an inner Exception");
            }
            catch (Exception ex1)
            {
                try
                {
                    throw new AggregateException("I am another inner Exception", ex1);
                }
                catch (Exception ex2)
                {
                    try
                    {
                        throw new Exception("I am an Exception", ex2);
                    }
                    catch (Exception ex3)
                    {
                        logger.Error(ex3, "MyError with ex");
                    }
                }

            }

            logger.Info("I am a normal line");

            Console.ReadLine();
            LogManager.Cleanup();
            
        }
    }
}
