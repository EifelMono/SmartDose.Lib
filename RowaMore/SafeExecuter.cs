using System;
using System.Collections.Generic;
using System.Text;

namespace RowaMore
{
    public static class SafeExecuter
    {
        public static void Catcher(Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch { };
        }
    }
}
