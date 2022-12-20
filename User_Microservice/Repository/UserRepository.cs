using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User_Microservice.Contracts;
using User_Microservice.Entity.Dto;
using User_Microservice.Entity.Models;
using Newtonsoft.Json;

namespace User_Microservice.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _userContext;

        public UserRepository(UserContext userContext)
        {
            _userContext = userContext;
        }

        public bool IsEmailExists(string email)
        {
            return   _userContext.User.Any(term => term.EmailAddress == email);
        }

        public bool IsPhoneExists(string phone)
        {
            return _userContext.Phone.Any(term => term.PhoneNumber == phone);
        }

        public bool IsAddressExists(AddressDTO Address)
        {
            string address = JsonConvert.SerializeObject(Address);
            foreach (Address item in _userContext.Address)
            {
                string addressDTO = JsonConvert.SerializeObject(item); 
                if(addressDTO == address)
                {
                    return true;
                }
            }
            return false;
        }

        public Guid GetUserId(string email)
        {
            return _userContext.User.Where(item => item.EmailAddress == email).Select(term => term.Id).FirstOrDefault(); 

        }

        public Guid SaveUser(User user)
        {
            _userContext.User.Add(user);
            _userContext.SaveChanges();
            return user.Id;
        }

        public Tuple<string,string> IsLoginDetailsExists(LoginDTO loginDTO)
        {
            string password = _userContext.User.Where(item => item.EmailAddress == loginDTO.EmailAddress).Select(term => term.UserSecret.Password).FirstOrDefault();
            string role = _userContext.User.Where(item => item.EmailAddress == loginDTO.EmailAddress).Select(term => term.Role).FirstOrDefault();
           
            return new Tuple<string, string>(role,password);
        }

        public bool IsCardExists(string cardNo)
        {
            return _userContext.Card.Any(item => item.CardNo == cardNo);
        }

        public Guid SaveCard(Card card)
        {
            _userContext.Card.Add(card);
            _userContext.SaveChanges();
            return card.Id;
        }
    }
}
