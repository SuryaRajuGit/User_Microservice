using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class PaymentDetailsResponseDTO
    {
        ///<summary>
        /// List of card dtos
        ///</summary>
        public List<CardDTO> Card { get; set; }

        ///<summary>
        /// List of upi dtos
        ///</summary>
        public List<UpiDTO> Upi { get; set; }
    }
}
