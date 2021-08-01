using System;
using System.Collections.Generic;
using System.Text;

namespace IMCTaxService.Entities.TaxClientEntities
{
    public class TaxClientOrder
    {
        public TaxClientAddress FromAddress { get; set; }
        public TaxClientAddress ToAddress { get; set; }
        public decimal ShippingCost { get; set; }

        //assuming we'd have a mapping of customer id's from our sources to taxjar's- if any id was required. ommiting for now.
        //public string CustomerId { get; set; }

        //omitting this entirely as i'm unsure whether other tax clients would support it.
        //public ExemptionType ExemptionType { get; set; }
        public List<TaxClientAddress> NexusAddress { get; set; }
        public List<TaxClientPurchase> Purchases { get; set; }
    }

    public class TaxClientPurchase
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal PriceUSD { get; set; }
        public decimal DiscountUSD { get; set; }
        public decimal SalesTaxUSD { get; set; }
    }

    public class TaxClientAddress
    {
        public string Country { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }

    public enum ExemptionType
    {
        Wholesale,
        Government,
        Marketplace,
        Other,
        NonExempt
    }
}
