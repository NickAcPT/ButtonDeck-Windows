using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Networking.Attributes
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class ArchitectureAttribute : Attribute
    {
        readonly PacketArchitecture architecture;

        public ArchitectureAttribute(PacketArchitecture architecture)
        {
            this.architecture = architecture;
        }

        public PacketArchitecture Architecture {
            get { return architecture; }
        }
    }

    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    sealed class ServerOnlyAttribute : Attribute
    {
    }

    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    sealed class ClientOnlyAttribute : Attribute
    {
    }

}
