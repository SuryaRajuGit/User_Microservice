using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Models
{
    public class Address : BaseModel
    {
        ///<summary>
        /// Foregin key User id
        ///</summary>
        public Guid UserId { get; set; }
        public User User { get; set; }

        ///<summary>
        /// Address Line1
        ///</summary>
        public string Line1 { get; set; }

        ///<summary>
        /// Address line 2
        ///</summary>
        public string Line2 { get; set; }

        ///<summary>
        /// User Address city
        ///</summary>
        public string City { get; set; }

        ///<summary>
        /// User Address zipcode
        ///</summary>
        public string Zipcode { get; set; }

        ///<summary>
        /// User Address statename
        ///</summary>
        public string StateName { get; set; }

        ///<summary>
        /// User Address country
        ///</summary>
        public string Country { get; set; }

        ///<summary>
        /// User Address type example: WORK
        ///</summary>
        public string Type { get; set; }

       
    }
}
