using AutoMapper;
using AutoMapper.Configuration;
using IMCTaxService.Entities.AutomapperConverters;
using IMCTaxService.Entities.ServiceDTO;
using IMCTaxService.Entities.TaxClientEntities;
using IMCTaxService.Entities.TaxJarDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMCTaxService
{
    public class AutoMapperServiceProfile : Profile
    {
        public AutoMapperServiceProfile()
        {
            CreateMap<TaxJarRate, TaxClientRate>();
            CreateMap<TaxClientRate, TaxRateResponse>();
            CreateMap<TaxClientOrder, TaxJarOrder>().ConvertUsing(new TaxClientOrder_TaxJarOrderCustomConverter());

            CreateMap<OrderRequest, TaxClientOrder>();
            CreateMap<Address, TaxClientAddress>();
            CreateMap<Purchase, TaxClientPurchase>();

            CreateMap<TaxJarTaxResponse, TaxClientOrderTax>().ConvertUsing(new TaxJarTaxResponse_OrderTaxCustomConverter());

            CreateMap<TaxClientOrderTax, OrderResponse>();
            CreateMap<TaxClientJurisdiction, Jurisdiction>();
        }
    }
}
