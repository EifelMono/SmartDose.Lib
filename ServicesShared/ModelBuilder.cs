using System;
using System.Collections.Generic;
using System.Text;

#if MasterData10000
namespace MasterData10000
#elif Settings10000
namespace Settings10000
#elif MasterData9002
namespace MasterData9002
#else
namespace ServicesShared
#endif
{
    public class ModelBuilder
    {
        public ModelBuilder(ServiceClientBase client)
        {
            Client = client;
        }

        protected ServiceClientBase Client { get; set; }

        protected Type ModelType { get; set; }

        protected bool TableOnlyFlag { get; set; } = false;
        protected bool _DebugInfoFlag { get; set; } = false;
        protected bool DebugInfoFlag
        {
            get => DebugInfoAllFlag ? DebugInfoAllFlag : _DebugInfoFlag;
            set => _DebugInfoFlag = value;
        }

        private static bool DebugInfoAllFlag { get; set; } = false;

        public static void SwitchDebugInfoAll(bool flag)
            => DebugInfoAllFlag = flag;
    }
}
