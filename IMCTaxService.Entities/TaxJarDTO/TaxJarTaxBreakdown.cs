using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMCTaxService.Entities.TaxJarDTO
{
	public class TaxJarTaxBreakdown : TaxJarBreakdown
	{
		[JsonProperty("state_tax_rate")]
		public decimal StateTaxRate { get; set; }

		[JsonProperty("state_tax_collectable")]
		public decimal StateTaxCollectable { get; set; }

		[JsonProperty("county_tax_collectable")]
		public decimal CountyTaxCollectable { get; set; }

		[JsonProperty("city_tax_collectable")]
		public decimal CityTaxCollectable { get; set; }

		[JsonProperty("special_district_taxable_amount")]
		public decimal SpecialDistrictTaxableAmount { get; set; }

		[JsonProperty("special_tax_rate")]
		public decimal SpecialDistrictTaxRate { get; set; }

		[JsonProperty("special_district_tax_collectable")]
		public decimal SpecialDistrictTaxCollectable { get; set; }

		[JsonProperty("shipping")]
		public TaxJarTaxBreakdownShipping Shipping { get; set; }

		[JsonProperty("line_items")]
		public List<TaxBreakdownLineItem> LineItems { get; set; }
	}
}
