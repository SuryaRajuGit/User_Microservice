using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User_Microservice.Entity.Models;

namespace User_Microservice.Entity.Dto
{
    public class CheckOutResponseDTO
    {
        public Address Address { get; set; }

        public Payment Payment { get; set; }
    }
}
