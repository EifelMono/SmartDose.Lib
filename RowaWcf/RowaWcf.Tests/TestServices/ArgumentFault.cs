using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RowaWcf.Tests.TestServices
{
    [DataContract]
    public class ArgumentFault
    {
        [DataMember]
        public string ArgumentName { get; set; }

        public ArgumentFault(string argumentName)
        {
            ArgumentName = argumentName;
        }
    }
}
