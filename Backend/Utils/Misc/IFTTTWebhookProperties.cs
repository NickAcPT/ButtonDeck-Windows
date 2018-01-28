using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Utils.Misc
{
    public class IFTTTWebhookProperties
    {
        public string Value1 { get; set; } = "";
        public string Value2 { get; set; } = "";
        public string Value3 { get; set; } = "";

        public string ToJson()
        {
            dynamic obj = new ExpandoObject();
            obj.value1 = Value1;
            obj.value2 = Value2;
            obj.value3 = Value3;
            return JsonConvert.SerializeObject(obj);
        }
    }
}
