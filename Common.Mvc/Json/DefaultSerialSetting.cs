using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Mvc
{
    public static class DefaultJsonSerializerSettings
    {
        public static JsonSerializerSettings Default;
        static DefaultJsonSerializerSettings()
        {
            Default = new JsonSerializerSettings();
            Default.ContractResolver = new LowerCasePropertyNamesContractResolver();
            Default.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            Default.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //Default.NullValueHandling = NullValueHandling.Ignore;
        }
    }
}
