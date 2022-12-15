using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User_Microservice.Entity.Dto;
using User_Microservice.Entity.Models;

namespace User_Microservice.Contracts
{
    public interface IUserRepository
    {
        public bool IsEmailExists(string email);

        public bool IsPhoneExists(string phone);

        public bool IsAddressExists(AddressDTO Address);

        public Guid SaveUser(User user);

        public Tuple<string, string> IsLoginDetailsExists(LoginDTO loginDTO);

        public Guid GetUserId(string email);

        public bool IsCardExists(string cardNo);

        public Guid SaveCard(Card cardDTO);
    }
}
