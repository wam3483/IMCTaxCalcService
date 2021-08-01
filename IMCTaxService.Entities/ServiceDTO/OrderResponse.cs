using System;
using System.Collections.Generic;
using System.Text;

namespace IMCTaxService.Entities.ServiceDTO
{
    public class OrderResponse
    {
        public decimal TotalUSD { get; set; }
        public decimal ShippingUSD { get; set; }
        public decimal TaxableAmountUSD { get; set; }
        public decimal SalesTaxUSD { get; set; }
        public Jurisdiction Jurisdiction { get; set; }
    }

    public class Jurisdiction
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string County { get; set; }
        public string City { get; set; }
    }
}
