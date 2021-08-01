using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMCTaxService.Proxy
{
    public class JsonConverter : IJsonConvert
    {
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
