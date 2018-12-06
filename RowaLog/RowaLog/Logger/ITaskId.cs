using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rowa.Lib.Log
{
    /// <summary>
    /// Task identifier interface that can be used for detailed task specific logging.
    /// </summary>
    public interface ITaskId
    {
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        string ToString();
    }
}
