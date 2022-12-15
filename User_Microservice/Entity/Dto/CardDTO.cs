using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class CardDTO
    {
        [Required]
        public string CardHolderName { get; set; }

        [Required]
        public string CardNo { get; set; }

        [Required]
        public string ExpiryDate { get; set; }
    }
}
