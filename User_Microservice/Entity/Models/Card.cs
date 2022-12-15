using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Models
{
    public class Card
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User  { get; set; }

        public string CardHolderName { get; set; }

        public string CardNo { get; set; }

        public string ExpiryDate { get; set; }
    }
}
