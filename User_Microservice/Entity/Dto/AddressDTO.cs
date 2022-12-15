using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class AddressDTO
    {
        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string City { get; set; }

        public string Zipcode { get; set; }

        public string StateName { get; set; }

        public string Country { get; set; }

        public string Type { get; set; }
    }
}
