using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMCTaxService.Entities.TaxJarDTO
{

    public class TaxJarTaxJurisdictions
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("county")]
        public string County { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }
    }
}
