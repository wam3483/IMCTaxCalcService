using AutoMapper;
using IMCTaxService.Entities.AppSettings;
using IMCTaxService.Entities.TaxClientEntities;
using IMCTaxService.Entities.TaxJarDTO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMCTaxService.Proxy
{
    public class TaxJarTaxClient : ITaxClient
    {
        private readonly IMapper _mapper;
        private readonly IRestClient _client;
        private readonly IJsonConvert _jsonConvert;

        public TaxJarTaxClient(IMapper mapper, IRestClient client, IJsonConvert jsonConvert)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _jsonConvert = jsonConvert ?? throw new ArgumentNullException(nameof(jsonConvert));
        }

        public TaxClientOrderTax GetTaxesForOrder(TaxClientOrder order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            if (order.FromAddress == null)
            {
                throw new ArgumentNullException(nameof(TaxClientOrder.FromAddress));
            }
            if (order.ToAddress == null)
            {
                throw new ArgumentNullException(nameof(TaxClientOrder.ToAddress));
            }
            var orderRequest = _mapper.Map<TaxJarOrder>(order);

            string orderRequestJson = _jsonConvert.Serialize(orderRequest);
            IRestRequest request = new RestRequest($"/taxes", Method.POST, DataFormat.Json);
            request.AddParameter("application/json;", orderRequestJson, ParameterType.RequestBody);
            var response = _client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string responseJson = response.Content;
                var taxJarResponseDTO = _jsonConvert.Deserialize<TaxJarTaxResponse>(responseJson);
                var taxClientResponse = _mapper.Map<TaxClientOrderTax>(taxJarResponseDTO);
                return taxClientResponse;
            }

            throw new ResponseException(response);
        }

        public TaxClientRate GetRates(Location location)
        {
            if (location == null)
            {
                throw new ArgumentNullException(nameof(location));
            }
            if (string.IsNullOrWhiteSpace(location.Zip))
            {
                throw new ArgumentException("Missing zipcode");
            }
            if (string.IsNullOrWhiteSpace(location.Country))
            {
                throw new ArgumentException("Missing country");
            }
            if (location.Country.Length != 2)
            {
                throw new ArgumentException("Invalid country, must be 2 characters");
            }
            IRestRequest request = new RestRequest($"/rates/{location.Zip}", Method.GET);
            request = request
                .AddQueryParameterIfNotNullOrWhitespace("country", location.Country)
                .AddQueryParameterIfNotNullOrWhitespace("city", location.City)
                .AddQueryParameterIfNotNullOrWhitespace("street", location.Street)
                .AddQueryParameterIfNotNullOrWhitespace("state", location.State);

            var response = _client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = response.Content;
                var rateResponse = _jsonConvert.Deserialize<TaxJarRateResponse>(json);

                var result = _mapper.Map<TaxClientRate>(rateResponse.Rate);
                return result;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                throw new ResponseException(response);
            }
        }
    }

    public class ResponseException : Exception
    {
        public ResponseException(IRestResponse response) :
            base($"Unexpected response from vendor API. StatusCode='{response.StatusCode}'\nResponse='{response.Content}'")
        {
        }
    }

    public static class RestRequestExtensions
    {
        public static IRestRequest AddQueryParameterIfNotNullOrWhitespace(this IRestRequest request, string name, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return request.AddQueryParameter(name, value);
            }
            return request;
        }
    }
}
