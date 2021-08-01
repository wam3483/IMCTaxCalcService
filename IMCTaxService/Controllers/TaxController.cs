using AutoMapper;
using IMCTaxService.Entities.AppObjects;
using IMCTaxService.Entities.ServiceDTO;
using IMCTaxService.Entities.TaxClientEntities;
using IMCTaxService.Proxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace IMCTaxService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TaxController : ControllerBase
    {
        private readonly IFactory<ITaxClient, UserType> _taxClientFactory;
        private readonly ILogger<TaxController> _logger;
        private readonly IMapper _mapper;

        //Using factory pattern and .net core DI framework, mixing DI strategies unfortunately.  Could not find a simple way to do conditional injection
        //within .net core DI framework, so fell back to factory pattern. conditional logic lives in factory.
        public TaxController(IFactory<ITaxClient, UserType> taxClientFactory, ILogger<TaxController> logger, IMapper mapper)
        {
            _taxClientFactory = taxClientFactory ?? throw new ArgumentNullException(nameof(taxClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private ITaxClient GetTaxClient()
        {
            UserType userType = GetUserType();
            var taxClient = _taxClientFactory.GetInstance(userType);
            return taxClient;
        }

        /// <summary>
        /// Returns tax rates for a location.
        /// </summary>
        /// <param name="country">2 digit country code e.g. US, CA</param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="street"></param>
        /// <param name="zipcode"></param>
        /// <returns></returns>
        [HttpGet("TaxRates")]
        [SwaggerResponse(httpStatusCode: HttpStatusCode.OK, responseType: typeof(TaxRateResponse))]
        [SwaggerResponse(httpStatusCode: HttpStatusCode.NotFound, responseType: typeof(string))]
        [SwaggerResponse(httpStatusCode: HttpStatusCode.BadRequest, responseType: typeof(string))]
        [SwaggerResponse(httpStatusCode: HttpStatusCode.InternalServerError, responseType: typeof(string))]
        public IActionResult TaxRates([BindRequired] string country, string city, string state, string street, [BindRequired] string zipcode)
        {
            //i'd not log here if there was some type of api gateway tracking requests/responses, but we don't know that.
            _logger.LogTrace($"{nameof(TaxController)}.TaxRates invocation: country='{country}' city='{city}' state='{state}' street='{street}' zipcode='{zipcode}'");

            //could have used custom validation, or attribute driven validation here instead.
            if (string.IsNullOrWhiteSpace(zipcode))
            {
                return BadRequest("Invalid zipcode");
            }
            if (string.IsNullOrWhiteSpace(country) || country.Length != 2)
            {
                return BadRequest("Invalid country");
            }
            //violation of automapper pattern
            //could not find a way i was happy with to bind query params to a DTO automatically. KISS it is.
            var location = new Location()
            {
                City = city,
                Country = country,
                Street = street,
                Zip = zipcode,
                State = state,
            };
            var taxClient = GetTaxClient();

            var rates = taxClient.GetRates(location);
            if (rates == null)
            {
                return NotFound("No results found");
            }

            var result = _mapper.Map<TaxRateResponse>(rates);
            return Ok(result);
        }

        //This should really be a GET operation but there are too many parameters to pass via query string, and passing a body in a GET request isn't really supposed
        //to be supported.  So POST it is.
        /// <summary>
        /// Returns sales tax to collect for an order.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Order")]
        [SwaggerResponse(httpStatusCode: HttpStatusCode.OK, responseType: typeof(OrderResponse))]
        [SwaggerResponse(httpStatusCode: HttpStatusCode.BadRequest, responseType: typeof(string))]
        [SwaggerResponse(httpStatusCode: HttpStatusCode.InternalServerError, responseType: typeof(string))]
        public IActionResult OrderTax(OrderRequest request)
        {
            //i'd not log here if there was some type of api gateway tracking requests/responses, but we don't know that.
            _logger.LogTrace($"{nameof(TaxController)}.OrderTax invocation: {request}");

            //should really have opted for custom validation attributes here on request DTO...
            if (request == null)
            {
                return BadRequest("missing request");
            }
            if (request.FromAddress == null)
            {
                return BadRequest($"missing FromAddress");
            }
            if (request.ToAddress == null)
            {
                return BadRequest("Missing ToAddress");
            }
            if (string.IsNullOrWhiteSpace(request.FromAddress.Country) || request.FromAddress.Country.Length != 2)
            {
                return BadRequest($"invalid country");
            }
            if (string.IsNullOrWhiteSpace(request.ToAddress.Country) || request.ToAddress.Country.Length != 2)
            {
                return BadRequest($"invalid country");
            }
            var taxClient = GetTaxClient();

            var taxClientRequest = _mapper.Map<TaxClientOrder>(request);
            TaxClientOrderTax taxClientResponse = taxClient.GetTaxesForOrder(taxClientRequest);
            var response = _mapper.Map<OrderResponse>(taxClientResponse);
            return Ok(response);
        }

        //i'd resolve usertype by examining claims on oauth token here, or by whatever mechanism was required
        //hardcoded for our purposes for now.
        private UserType GetUserType()
        {
            return UserType.UserA;
        }
    }
}
