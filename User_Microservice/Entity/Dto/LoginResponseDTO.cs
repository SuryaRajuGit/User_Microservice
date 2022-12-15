using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Dto
{
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; }

        public string TokenType { get; set; }
    }
}
