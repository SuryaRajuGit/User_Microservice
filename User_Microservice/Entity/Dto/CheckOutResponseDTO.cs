using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User_Microservice.Entity.Models;

namespace User_Microservice.Entity.Dto
{
    public class CheckOutResponseDTO
    {
        ///<summary>
        /// Address of the user
        ///</summary>
        public Address Address { get; set; }

        ///<summary>
        /// Payment of the user
        ///</summary>
        public Payment Payment { get; set; }
    }
}
