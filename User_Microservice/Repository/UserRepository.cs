using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User_Microservice.Contracts;
using User_Microservice.Entity.Dto;
using User_Microservice.Entity.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
namespace User_Microservice.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _userContext;

        public UserRepository(UserContext userContext)
        {
            _userContext = userContext;
        }

        ///<summary>
        /// Checks wheather the email exists.
        ///</summary>
        ///<param name="email">email address of the user</param>
        ///<return>bool</return>
        public bool IsEmailExists(string email)
        {
            return _userContext.User.Any(term => term.EmailAddress == email && term.IsActive );
        }

        ///<summary>
        /// Checks wheather phone number exists.
        ///</summary>
        //////<param name="phone">email address of the user</param>
        ///<return>bool</return>
        public bool IsPhoneExists(string phone)
        {
            return _userContext.Phone.Any(term => term.PhoneNumber == phone && term.IsActive);
        }

        ///<summary>
        /// Checks wheather Address exists.
        ///</summary>
        ///<return>bool</return>
        public bool IsAddressExists(AddressDTO Address)
        {
            string address = JsonConvert.SerializeObject(Address);
            foreach (Address item in _userContext.Address.Where(sel => sel.IsActive))
            {
                AddressDTO addressDTO1 = new AddressDTO()
                {
                    Line1=item.Line1,
                    Line2=item.Line2,
                    City=item.City,
                    Zipcode=item.Zipcode,
                    StateName = item.StateName,
                    Country =item.Country,
                    Type=item.Type
                };
                string addressDTO = JsonConvert.SerializeObject(addressDTO1); 
                if(addressDTO == address)
                {
                    return true;
                }
            }
            return false;
        }

        ///<summary>
        ///  Gets user id using Email address.
        ///</summary>
        ///<return>User Guid</return>
        public Guid GetUserId(string email)
        {
           
            
            return _userContext.User.Where(item => item.EmailAddress == email && item.IsActive).Select(term => term.Id).FirstOrDefault(); 

        }

        ///<summary>
        /// Saves user details in the database.
        ///</summary>
        ///<return>User Guid</return>
        public Guid SaveUser(User user)
        {
            user.CreatedDate = DateTime.Now;
            _userContext.User.Add(user);
            _userContext.SaveChanges();
            return user.Id;
        }

        ///<summary>
        /// Checks wheather the email exists.
        ///</summary>
        ///<return>bool</return>
        public Tuple<string,string> IsLoginDetailsExists(LoginDTO loginDTO)
        {
            string password = _userContext.User.Where(item => item.EmailAddress == loginDTO.EmailAddress).Select(term => term.UserSecret.Password).FirstOrDefault();
            string role = _userContext.User.Where(item => item.EmailAddress == loginDTO.EmailAddress).Select(term => term.Role).FirstOrDefault();
           
            return new Tuple<string, string>(role,password);
        }

        ///<summary>
        /// Checks wheather payment card exists.
        ///</summary>
        ///<return>bool</return>
        public bool IsCardExists(string cardNo,Guid id)
        {
            return _userContext.Payment.Where(find => find.UserId == id && find.IsActive).Any(term => term.CardNo == cardNo);
        }

        ///<summary>
        /// Checks wheather the email exists.
        ///</summary>
        ///<return>Card id</return>
        public Guid SaveCard(Payment card)
        {
            _userContext.Payment.Add(card);
            _userContext.SaveChanges();
            return card.Id;
        }

        ///<summary>
        /// Checks wheather the user email exists.
        ///</summary>
        ///<return>bool</return>
        public bool IsUpdateUserEmailExists(string emailAddress,Guid userId)
        {
            foreach (User item in _userContext.User.Where(user => user.Id != userId))
            {
                if(item.EmailAddress == emailAddress)
                {
                    return false;
                }
            }
            return true;
        }

        ///<summary>
        /// Checks wheather the phone exists.
        ///</summary>
        ///<return>bool</return>
        public bool IsUpdateUserPhoneExists(string phoneNumber, Guid userId)
        {
            foreach (User item in _userContext.User.Include(term => term.Phone).Where(user => user.Id != userId))
            {
                if (item.Phone.PhoneNumber == phoneNumber)
                {
                    return false;
                }
            }
            return true;
        }

        ///<summary>
        /// Checks wheather the Address exists.
        ///</summary>
        ///<return>bool</return>
        public bool IsUpdateUserAddressExists(Address address, Guid userId)
        {
            string userAddress = JsonConvert.SerializeObject(address);
            foreach (Address item in _userContext.Address.Where(find => find.UserId != userId))
            {
                Address address1 = new Address()
                {
                    Line1 = item.Line1,
                    Line2 = item.Line2,
                    City = item.City,
                    StateName = item.StateName,
                    Country = item.Country,
                    Zipcode = item.Zipcode,
                    Type = item.Type
                };               
                string addressDTO = JsonConvert.SerializeObject(address1);
                if (addressDTO == userAddress)
                {
                    return true;
                }
            }
            return false;
        }

        ///<summary>
        /// Checks wheather the user exists.
        ///</summary>
        ///<return>bool</return>
        public bool IsUserExist(Guid userId)
        {
            return _userContext.User.Any(find => find.Id == userId);
        }

        ///<summary>
        /// Saves user details.
        ///</summary>
        public void SaveUpdateUser(User user)
        {
            _userContext.User.Update(user);
            _userContext.SaveChanges();
        }

        ///<summary>
        /// Checks wheather the user exists.
        ///</summary>
        ///<return>User dto</return>
        public User GetUserDetails(Guid id)
        {
            return _userContext.User
                .Include(sel => sel.Payment.Where(sel => sel.IsActive))
                .Where(sel => sel.Id == id && sel.IsActive).FirstOrDefault();
        }

        ///<summary>
        /// Deletes user account
        ///</summary>
        ///<return>bool</return>
        public bool DeleteUser(Guid id)
        {
            List<User> userList = _userContext.User.ToList();
            bool isUserExist = _userContext.User.Any(find => find.Id == id);
            if(isUserExist)
            {
                User user = _userContext.User.Include(term => term.Payment).Include(term=>term.Address)
                    .Include(term => term.UserSecret).Include(term =>term.Payment)
                    .Where(find => find.Id == id).FirstOrDefault();
                _userContext.User.Remove(user);
                _userContext.SaveChanges();
                return true;
            }
            return false;
        }
        ///<summary>
        /// Gets all the user card details
        ///</summary>
        ///<return>List<Payment></return>
        public List<Payment> GetCardDetails(Guid id)
        {
            List<Payment> cardList = _userContext.Payment.Where(find => find.UserId == id).ToList();
            return cardList;
        }
        ///<summary>
        /// Checks is user details exist or not
        ///</summary>
        ///<return>bool</return>
        public bool IsUserPaymentDetailsExist(Guid cardId,Guid userId)
        {
            User user = _userContext.User.Include(src => src.Payment).Where(find => find.Id == userId).First();
            return user.Payment.Any(term => term.Id == cardId);
        }
        ///<summary>
        /// Saves user card details and return bool value
        ///</summary>
        ///<return>bool</return>
        public bool isCardDetailsExist(Payment card)
        {
            List<Payment> cardList =  _userContext.Payment.Where(find => find.UserId == card.UserId && find.Id != card.Id).ToList();
            bool isCardExist = cardList.Any(find => find.CardNo == card.CardNo);
            if(!isCardExist)
            {
                _userContext.Payment.Update(card);
                _userContext.SaveChanges();
                return false;
            }
            return true;
        }
        ///<summary>
        ///Gets User details 
        ///</summary>
        ///<return>User</return>
        public User GetUserAccount(Guid id)
        {
            return _userContext.User.Include(src => src.Phone).Include(src => src.UserSecret).Include(src => src.Address)
                .Include(src => src.Payment).Where(find => find.Id == id).FirstOrDefault();

        }
        ///<summary>
        /// Checks Upi exist or not
        ///</summary>
        ///<return>bool</return>
        public bool CheckUpi(Guid id)
        {
            return _userContext.Payment.Any(find => find.Id == id);
        }

        ///<summary>
        /// Gets UPI details of the user
        ///</summary>
        ///<return>List<Payment></return>
        public List<Payment> GetUpiDetails(Guid userId)
        {
            return _userContext.Payment.Where(find => find.UserId == userId && find.ExpiryDate == null).ToList();
        }

        ///<summary>
        /// Saves Payment card details
        ///</summary>
        public void SaveUpdateCard(Payment card)
        {
            _userContext.Payment.Update(card);
            _userContext.SaveChanges();
        }

        ///<summary>
        /// Checks address id exist or not
        ///</summary>
        ///<return>bool</return>
        public bool IsAddressIdExist(Guid addressId)
        {
            return _userContext.Address.Any(find =>find.Id ==addressId);
        }

        ///<summary>
        /// Checks Payment id exist or not
        ///</summary>
        ///<return>bool</return>
        public bool IsPaymentIdExist(Guid paymentId)
        {
            return _userContext.Payment.Any(find =>find.Id == paymentId);
        }
        ///<summary>
        /// Gets Addres of the user
        ///</summary>
        ///<return>Address</return>
        public Address GetAddress(Guid addressId)
        {
            return _userContext.Address.Where(find => find.Id == addressId).First();
        }

        ///<summary>
        /// Gets Payment details of the user
        ///</summary>
        ///<return>Payment</return>
        public Payment GetPaymentDetails(Guid paymentId)
        {
            return _userContext.Payment.Where(find => find.Id == paymentId).First();
        }
    }
}
