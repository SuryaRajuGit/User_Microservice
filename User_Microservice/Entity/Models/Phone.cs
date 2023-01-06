﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Models
{
    public class Phone
    {
        ///<summary>
        /// Phone id
        ///</summary>
        public Guid Id { get; set; }

        ///<summary>
        /// Foregin key user id
        ///</summary>
        public Guid UserId { get; set; }
        public User User { get; set; }

        ///<summary>
        /// Phone number of the user 
        ///</summary>
        public string PhoneNumber { get; set; }

        ///<summary>
        /// User phone number type
        ///</summary>
        public string Type { get; set; }
    }
}
