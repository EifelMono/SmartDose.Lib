using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDose.WcfLib
{
    public class ModelBuilder
    {
        public ModelBuilder(IServiceClientModel client)
        {
            Client = client;
        }

        protected IServiceClientModel Client { get; set; }

        protected Type ModelType { get; set; }

        #region Debug

        private static bool DebugInfoFlagAll { get; set; } = false;

        public static void SetDebugInfoFlagAll(bool on)
            => DebugInfoFlagAll = on;

        protected bool _DebugInfoFlag { get; set; } = false;
        protected bool DebugInfoFlag
        {
            get => DebugInfoFlagAll ? DebugInfoFlagAll : _DebugInfoFlag;
            set => _DebugInfoFlag = value;
        }

        #endregion

        protected bool TableOnlyFlag { get; set; } = false;
    }
}
