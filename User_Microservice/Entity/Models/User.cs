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
        ///<summary>
        /// User id
        ///</summary>
        public Guid Id { get; set; }

        ///<summary>
        /// first name of the user
        ///</summary>
        public string FirstName { get; set; }

        ///<summary>
        /// last name of the user
        ///</summary>
        public string LastName { get; set; }

        ///<summary>
        /// Email address of the user
        ///</summary>
        public string EmailAddress { get; set; }

        ///<summary>
        /// Role of the user account example: Admin or user
        ///</summary>
        public string Role { get; set; }

        ///<summary>
        /// Address of the user 
        ///</summary>
        public Address Address { get; set; }

        ///<summary>
        /// Card of the user 
        ///</summary>
        public ICollection<Payment> Payment { get; set; }

        ///<summary>
        /// phone details of the user
        ///</summary>
        public Phone Phone { get; set; }

        ///<summary>
        /// Password details of the user 
        ///</summary>
        public UserSecret UserSecret { get; set; }
    }
}
