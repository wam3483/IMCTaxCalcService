using System;
using System.Collections.Generic;
using System.Text;

namespace IMCTaxService.Entities.TaxClientEntities
{
    public class TaxClientOrderTax
    {
        public decimal TotalUSD { get; set; }
        public decimal ShippingUSD { get; set; }
        public decimal TaxableAmountUSD { get; set; }
        public decimal SalesTaxUSD { get; set; }
        public TaxClientJurisdiction Jurisdiction { get; set; }
        //public JurisdictionBreakdown JurisdictionBreakdown { get; set; }
    }
}
