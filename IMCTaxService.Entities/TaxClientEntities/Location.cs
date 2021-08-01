using System;
using System.Collections.Generic;
using System.Text;

namespace IMCTaxService.Entities.TaxClientEntities
{
    public class Location
    {
        public string Country { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

        public override string ToString()
        {
            return $"{nameof(Zip)}='{Zip}' {nameof(Country)}='{Country}' {nameof(State)}='{State}' {nameof(City)}='{City}' {nameof(Street)}='{Street}'";
        }
    }
}
