using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMCTaxService.Entities.TaxJarDTO
{
    public class TaxJarRateResponse
    {
        [JsonProperty("rate")]
        public TaxJarRate Rate { get; set; }
    }

    public class TaxJarRate
    {
        [JsonProperty("zip")]
        public string Zip { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("country_rate")]
        public double CountryRate { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("state_rate")]
        public double StateRate { get; set; }
        [JsonProperty("county")]
        public string County { get; set; }
        [JsonProperty("county_rate")]
        public string CountyRate { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("city_rate")]
        public double CityRate { get; set; }
        [JsonProperty("combined_district_rate")]
        public double CombinedDistrictRate { get; set; }
        [JsonProperty("combined_rate")]
        public double CombinedRate { get; set; }
        [JsonProperty("freight_taxable")]
        public bool FreightTaxable { get; set; }
    }
}
