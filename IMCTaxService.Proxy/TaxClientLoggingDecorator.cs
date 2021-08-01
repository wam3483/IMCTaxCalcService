using IMCTaxService.Entities.TaxClientEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMCTaxService.Proxy
{
    public class TaxClientLoggingDecorator : ITaxClient
    {
        private readonly ITaxClient _client;
        private readonly ILogger _logger;

        public TaxClientLoggingDecorator(ILogger logger, ITaxClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public TaxClientRate GetRates(Location location)
        {
            _logger.LogTrace($"{nameof(TaxClientLoggingDecorator)}.GetRates request {location}");
            var rate = _client.GetRates(location);
            _logger.LogTrace($"{nameof(TaxClientLoggingDecorator)}.GetRates response {rate}");
            return rate;
        }

        public TaxClientOrderTax GetTaxesForOrder(TaxClientOrder order)
        {
            _logger.LogTrace($"{nameof(TaxClientLoggingDecorator)}.GetTaxesForOrder request {order}");
            var orderTax = _client.GetTaxesForOrder(order);
            _logger.LogTrace($"{nameof(TaxClientLoggingDecorator)}.GetTaxesForOrder response {orderTax}");
            return orderTax;
        }
    }
}
