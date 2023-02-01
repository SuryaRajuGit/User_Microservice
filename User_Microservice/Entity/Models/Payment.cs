using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Models
{
    public class Payment : BaseModel
    {


        ///<summary>
        /// Forgein key User id
        ///</summary>
        public Guid UserId { get; set; }
        public User User  { get; set; }

        ///<summary>
        /// Name of the user on the card
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        /// Card Number
        ///</summary>
        public string CardNo { get; set; }

        ///<summary>
        /// Cart Expiry date.
        ///</summary>
        public string ExpiryDate { get; set; }

        
    }
}
