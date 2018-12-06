using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rowa.Lib.Log
{
    /// <summary>
    /// This Enum represents the Different Types of Template Values
    /// </summary>
    internal enum LogConfigTemplateValueType
    {
        Value,
        Constant
    }

    /// <summary>
    /// This Class represents a Template Value in Log Templates
    /// </summary>
    class LogConfigTemplateValue
    {
        /// <summary>
        /// Type of the Template Value
        /// </summary>
        public LogConfigTemplateValueType Type;

        /// <summary>
        /// Name/Key of the Value
        /// </summary>
        public string Name;

        /// <summary>
        /// Formatstring to Convert Value to String
        /// </summary>
        public string Format;

        /// <summary>
        /// The actual Value
        /// </summary>
        public string Value;

        /// <summary>
        /// Returns the String Represantion of the Value
        /// </summary>
        /// <returns>Value as string</returns>
        public override string ToString()
        {
            return Value;
        }
    }
}
