using NickAc.Backend.Objects.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NickAc.Backend.Objects
{
    [XmlInclude(typeof(DynamicDeckItem))]
    public abstract class IDeckItem
    {
        
    }
}
