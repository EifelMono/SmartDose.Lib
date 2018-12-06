using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rowa.Lib.Log.Observer
{
    internal interface IObserver
    {
        /// <summary>
        /// Methods that is called when there is a Notification availble for the 
        /// Observer
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        void onNotify(Notification notification); 
    }
}
