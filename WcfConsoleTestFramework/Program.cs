using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rowa.Lib.Log;
using Rowa.Lib.Wcf.Configuration;
using WcfConsoleTestFramework.ServiceReference1;

namespace WcfConsoleTestFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            LogManager.Initialize("SmartDose", "MasterData");
            try
            {
                var _masterDataClient = new MasterDataClient(
                        WellKnownUrl.Get(WellKnownUrlType.SDMasterData, "127.0.0.1"),
                    new MasterDataCallbacks());

                _masterDataClient.ConnectionEstablished += MasterData_OnConnectionEstablished;
                _masterDataClient.ConnectionClosed += MasterData_ConnectionClosed;

                _masterDataClient.Events.OnMedicinesChanged += ServerNg_OnMedicinesChanged;
                _masterDataClient.Events.OnMedicinesDeleted += ServerNg_OnMedicinesDeleted;

                _masterDataClient.Start();
                _masterDataClient.Service.SubscribeForCallbacks();

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                LogManager.Cleanup();
            }

            Console.ReadLine();
        }

        private static void ServerNg_OnMedicinesDeleted(object arg1, string[] arg2)
        {
        }

        private static void ServerNg_OnMedicinesChanged(object arg1, Medicine[] arg2)
        {
            Console.WriteLine("Medicine changed");
        }

        private static void MasterData_ConnectionClosed(object sender, EventArgs e)
        {
            Console.WriteLine("MasterData_ConnectionClosed");
        }

        private static void MasterData_OnConnectionEstablished(object sender, EventArgs e)
        {
            Console.WriteLine("MasterData_OnConnectionEstablished");
        }
    }
}
