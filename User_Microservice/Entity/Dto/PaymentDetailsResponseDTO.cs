using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class PaymentDetailsResponseDTO
    {
        public List<CardDTO> Card { get; set; }

        public List<UpiDTO> Upi { get; set; }
    }
}
