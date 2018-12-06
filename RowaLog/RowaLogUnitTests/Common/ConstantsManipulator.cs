using AutoMapper;
using Rowa.Lib.Log.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowaLogUnitTests.Common
{
    internal class ConstantsManipulator : Globals
    {

        #region ------------- Fields -------------
        /// <summary>
        /// Object of LogConstants holding the default value
        /// can be used to reset the Values after Injecting them
        /// </summary>
        private LogConstants _defaultValue;
        #endregion

        #region ------------- Methods -------------
        /// <summary>
        /// Changes the Globals.Constants object with the Values given
        /// </summary>
        /// <param name="CleanupThreadIntervallMilliseconds">The new CleanupThreadIntervall, if not provided Default will be set</param>
        /// <param name="MaxLogFileSizeInBytes">The new MaxLogFileSize that should be set, it not provided default will be used</param>
        public void InjectValues(int cleanupThreadIntervallMilliseconds = 0, int maxLogFileSizeInBytes = 0)
        {
            _defaultValue = Globals.Constants;

            Mapper.Reset();

            Mapper.Initialize(x =>
            {
                x.CreateMap<LogConstants, DerivedConstants>();
            });

            DerivedConstants newConstants = Mapper.Map<DerivedConstants>(_defaultValue);

            if (cleanupThreadIntervallMilliseconds != 0)
            {
                newConstants.ChangeCleanupThreadIntervall(cleanupThreadIntervallMilliseconds);
            }
            if (maxLogFileSizeInBytes != 0)
            {
                newConstants.ChangeLogFileSize(maxLogFileSizeInBytes);
            }
            InjectConstants(newConstants);
        }

        /// <summary>
        /// Restes the Globals to the Default Value before 
        /// </summary>
        public void Reset()
        {
            InjectConstants(_defaultValue);
        }
        #endregion
    }

    internal class DerivedConstants : LogConstants
    {
        /// <summary>
        /// Changes the CleanupThreadIntervalMilliseconds to the given Value
        /// </summary>
        /// <param name="value"></param>
        public void ChangeCleanupThreadIntervall(int value)
        {
            _cleanupThreadIntervalMilliseconds = value;
        }

        /// <summary>
        /// Changes the MaxLogFileSizeInBytes to the given Value 
        /// </summary>
        /// <param name="value"></param>
        public void ChangeLogFileSize(int value)
        {
            _maxLogFileSizeInBytes = value;
        }

    }
}
