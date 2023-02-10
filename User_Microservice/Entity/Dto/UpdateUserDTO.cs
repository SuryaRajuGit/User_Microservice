using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class UpdateUserDTO
    {
        ///<summary>
        /// id of the user
        ///</summary>
        [Required]
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        ///<summary>
        /// Password of the user
        ///</summary>
        [JsonProperty(PropertyName = "password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$",
            ErrorMessage = "Passwords must be at least 8 characters, one upper case (A-Z)," +
            "one lower case (a-z),a number (0-9) and special character (e.g. !@#$%^&*)")]
        public string? Password { get; set; }

        ///<summary>
        /// First name of the user
        ///</summary>
        [JsonProperty(PropertyName = "first_name")]
        public string? FirstName { get; set; }

        ///<summary>
        /// Last name of the user
        ///</summary>
        [JsonProperty(PropertyName = "last_name")]
        public string? LastName { get; set; }

        ///<summary>
        /// email address of the user
        ///</summary>
        [JsonProperty(PropertyName = "email_address")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "Enter Valid email address")]
        public string? EmailAddress { get; set; }

        ///<summary>
        /// Phone number of the user
        ///</summary>
        [JsonProperty(PropertyName = "phone")]
        public UpdatePhoneDTO? Phone { get; set; }

        ///<summary>
        /// Address of the user
        ///</summary>
        [JsonProperty(PropertyName = "address")]
        public UpdateAddressDTO? Address { get; set; }


    }
}
