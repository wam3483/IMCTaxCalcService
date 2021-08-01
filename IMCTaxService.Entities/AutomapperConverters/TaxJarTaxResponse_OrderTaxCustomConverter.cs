using AutoMapper;
using IMCTaxService.Entities.TaxClientEntities;
using IMCTaxService.Entities.TaxJarDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMCTaxService.Entities.AutomapperConverters
{
    public class TaxJarTaxResponse_OrderTaxCustomConverter : ITypeConverter<TaxJarTaxResponse, TaxClientOrderTax>
    {
        public TaxClientOrderTax Convert(TaxJarTaxResponse source, TaxClientOrderTax destination, ResolutionContext context)
        {
            destination = destination ?? new TaxClientOrderTax();
            var tax = source.Tax;
            destination.TaxableAmountUSD = tax.TaxableAmount;
            destination.TotalUSD = tax.OrderTotalAmount;
            destination.ShippingUSD = tax.Shipping;
            destination.SalesTaxUSD = tax.AmountToCollect;

            destination.Jurisdiction = new TaxClientJurisdiction();
            destination.Jurisdiction.City = tax.Jurisdictions.City;
            destination.Jurisdiction.Country = tax.Jurisdictions.Country;
            destination.Jurisdiction.County = tax.Jurisdictions.County;
            destination.Jurisdiction.State = tax.Jurisdictions.State;

            return destination;
        }
    }
}
