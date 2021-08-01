using System;
using System.Collections.Generic;
using System.Text;

namespace IMCTaxService.Entities.ServiceDTO
{
    public class TaxRateResponse
    {
        public string Zip { get; set; }
        public string Country { get; set; }
        public double CountryRate { get; set; }
        public string State { get; set; }
        public double StateRate { get; set; }
        public string County { get; set; }
        public string CountyRate { get; set; }
        public string City { get; set; }
        public double CityRate { get; set; }
        public double CombinedDistrictRate { get; set; }
        public double CombinedRate { get; set; }
        public bool FreightTaxable { get; set; }
    }
}
