using AutoMapper;
using IMCTaxService.Entities.TaxClientEntities;
using IMCTaxService.Entities.TaxJarDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace IMCTaxService.Entities.AutomapperConverters
{
    public class TaxClientOrder_TaxJarOrderCustomConverter : ITypeConverter<TaxClientOrder, TaxJarOrder>
    {
        public TaxJarOrder Convert(TaxClientOrder source, TaxJarOrder destination, ResolutionContext context)
        {
            destination = destination ?? new TaxJarOrder();

            var fromAddress = source?.FromAddress;
            destination.FromCity = fromAddress?.City;
            destination.FromZip = fromAddress?.Zip;
            destination.FromCountry = fromAddress?.Country;
            destination.FromState = fromAddress?.State;
            destination.FromStreet = fromAddress?.Street;

            var toAddress = source?.ToAddress;
            destination.ToCity = toAddress?.City;
            destination.ToZip = toAddress?.Zip;
            destination.ToCountry = toAddress?.Country;
            destination.ToState = toAddress?.State;
            destination.ToStreet = toAddress?.Street;

            destination.ExemptionType = "non_exempt";

            destination.LineItems = source.Purchases.Select(s => new TaxJarLineItem()
            {
                Description = s.Description,
                Quantity = s.Quantity,
                UnitPrice = s.PriceUSD,
                Discount = s.DiscountUSD,
                SalesTax = s.SalesTaxUSD,
            })
            .ToList();

            return destination;
        }
    }
}
