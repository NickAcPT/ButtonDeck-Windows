using NickAc.Backend.Objects.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NickAc.Backend.Objects
{
    [XmlInclude(typeof(DynamicDeckFolder))]
    public abstract class IDeckFolder : IDeckItem
    {
        public abstract List<IDeckFolder> GetSubFolders();
        public abstract List<IDeckItem> GetDeckItems();
    }
}
