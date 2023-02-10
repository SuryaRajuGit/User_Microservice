using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class CheckOutCart
    {
        ///<summary>
        /// payment id 
        ///</summary>
        [JsonProperty(PropertyName = "payment_id")]
        public Guid PaymentId { get; set; }

        ///<summary>
        /// Address id
        ///</summary>
        [JsonProperty(PropertyName = "address_id")]
        public Guid AddressId { get; set; }
    }
}
