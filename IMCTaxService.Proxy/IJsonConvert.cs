using System;
using System.Collections.Generic;
using System.Text;

namespace IMCTaxService.Proxy
{
    public interface IJsonConvert
    {
        string Serialize(object obj);

        T Deserialize<T>(string json);
    }
}
