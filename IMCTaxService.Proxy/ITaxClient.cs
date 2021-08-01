using IMCTaxService.Entities.TaxClientEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMCTaxService.Proxy
{
    public interface ITaxClient
    {
        TaxClientOrderTax GetTaxesForOrder(TaxClientOrder order);
        TaxClientRate GetRates(Location location);
    }
}