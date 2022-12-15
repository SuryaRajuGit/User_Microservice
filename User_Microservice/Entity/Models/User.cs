using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Models
{
    public class User
    {
        public Guid Id { get; set; }


        public string FirstName { get; set; }


        public string LastName { get; set; }

        
        public string EmailAddress { get; set; }

        public string Role { get; set; }

        public Address Address { get; set; }

        public Card Card { get; set; }

        public Phone Phone { get; set; }

        public UserSecret UserSecret { get; set; }
    }
}
