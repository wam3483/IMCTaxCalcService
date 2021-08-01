using IMCTaxService.Entities.AppObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMCTaxService.Proxy
{
    public class TaxClientFactory : IFactory<ITaxClient, UserType>
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        public TaxClientFactory(ILogger<TaxClientFactory> logger, IServiceProvider serviceProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
        public ITaxClient GetInstance(UserType userType)
        {
            ITaxClient instance = CreateInstance(userType);
            return new TaxClientLoggingDecorator(_logger, instance);
        }

        //A true implementation here would return a different instance of ITaxClient based on usertype.
        //we only have one implementation, but there was mention in requirements of having a second instance in the future.
        private ITaxClient CreateInstance(UserType userType)
        {
            switch (userType)
            {
                case UserType.UserA:
                    return (ITaxClient)_serviceProvider.GetService(typeof(ITaxClient));
                default:
                    throw new NotImplementedException($"Have not implemented logic for usertype '{userType}'");
            }
        }
    }
}
