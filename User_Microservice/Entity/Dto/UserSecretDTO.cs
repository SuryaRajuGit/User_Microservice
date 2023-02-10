using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class UserSecretDTO
    {
        ///<summary>
        /// Password of the user
        ///</summary>
        public string Password { get; set; }
    }
}
