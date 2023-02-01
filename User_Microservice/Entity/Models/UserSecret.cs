using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Models
{
    public class UserSecret : BaseModel
    {


        public Guid UserId { get; set; }
        public User User { get; set; }

        ///<summary>
        /// Password of the user
        ///</summary>
        public string Password { get; set; }

    }
}
