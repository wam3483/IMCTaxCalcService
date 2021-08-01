using System;
using System.Collections.Generic;
using System.Text;

namespace IMCTaxService.Entities.TaxClientEntities
{
    public class JurisdictionBreakdown
    {
        public decimal TaxableAmountUSD { get; set; }
        public decimal TaxCollectableUSD { get; set; }
        public decimal CombinedTaxRate { get; set; }
        public decimal StateTaxableAmountUSD { get; set; }
        public decimal StateTaxRateUSD { get; set; }
        public decimal StateTaxCollectableUSD { get; set; }
        public decimal CountyTaxCollectableUSD{ get; set; }
        public decimal CityTaxableAmountUSD { get; set; }
        public decimal SpecialDistrictTaxableAmountUSD { get; set; }
        public decimal SpecialTaxRate { get; set; }
        public decimal SpecialDistrictTaxCollectable { get; set; }
        public List<TaxClientPurchase> Purchases { get; set; }
    }
}
