using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMCTaxService
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> exceptionLogger)
        {
            _logger = exceptionLogger ?? throw new ArgumentNullException(nameof(exceptionLogger));
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception.GetBaseException();
            _logger.LogError(exception, "uncaught exception");
        }
    }
}
