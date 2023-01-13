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
        ///<summary>
        /// Checks wheather the email exists.
        ///</summary>
        ///<param name="email">email address of the user</param>
        ///<return>bool</return>
        public bool IsEmailExists(string email);

        ///<summary>
        /// Checks wheather phone number exists.
        ///</summary>
        //////<param name="phone">email address of the user</param>
        ///<return>bool</return>
        public bool IsPhoneExists(string phone);

        ///<summary>
        /// Checks wheather Address exists.
        ///</summary>
        ///<return>bool</return>
        public bool IsAddressExists(AddressDTO Address);


        ///<summary>
        /// Saves user details in the database.
        ///</summary>
        ///<return>User Guid</return>
        public Guid SaveUser(User user);

        ///<summary>
        /// Checks wheather the email exists.
        ///</summary>
        ///<return>bool</return>
        public Tuple<string, string> IsLoginDetailsExists(LoginDTO loginDTO);

        ///<summary>
        ///  Gets user id using Email address.
        ///</summary>
        ///<return>User Guid</return>
        public Guid GetUserId(string email);

        ///<summary>
        /// Checks wheather payment card exists.
        ///</summary>
        ///<return>bool</return>
        public bool IsCardExists(string cardNo,Guid id);

        ///<summary>
        /// Checks wheather the email exists.
        ///</summary>
        ///<return>Card id</return>
        public Guid SaveCard(Payment cardDTO);

        ///<summary>
        /// Checks wheather the user email exists.
        ///</summary>
        ///<return>bool</return>
        public bool IsUpdateUserEmailExists(string emailAddress,Guid userId);

        ///<summary>
        /// Checks wheather the phone exists.
        ///</summary>
        ///<return>bool</return>
        public bool IsUpdateUserPhoneExists(string phoneNumber,Guid userId);

        ///<summary>
        /// Checks wheather the Address exists.
        ///</summary>
        ///<return>bool</return>
        public bool IsUpdateUserAddressExists(Address address,Guid userId);

        ///<summary>
        /// Checks wheather the user exists.
        ///</summary>
        ///<return>bool</return>
        public bool IsUserExist(Guid userId);

        ///<summary>
        /// Checks wheather the user exists.
        ///</summary>
        ///<return>User dto</return>
        public User GetUserDetails(Guid id);

        ///<summary>
        /// Saves user details.
        ///</summary>
        public void SaveUpdateUser(User user);

        ///<summary>
        /// Deletes user account
        ///</summary>
        ///<return>bool</return>
        public bool DeleteUser(Guid id);
        ///<summary>
        /// Gets all the user card details
        ///</summary>
        ///<return>List<Payment></return>
        public List<Payment> GetCardDetails(Guid id);

        ///<summary>
        /// Saves user card details and return bool value
        ///</summary>
        ///<return>bool</return>
        public bool isCardDetailsExist(Payment card);

        ///<summary>
        ///Gets User details 
        ///</summary>
        ///<return>User</return>
        public User GetUserAccount(Guid id);

        ///<summary>
        /// Checks Upi exist or not
        ///</summary>
        ///<return>bool</return>
        public bool CheckUpi(Guid id);

        ///<summary>
        /// Gets UPI details of the user
        ///</summary>
        ///<return>List<Payment></return>
        public List<Payment> GetUpiDetails(Guid userId);

        ///<summary>
        /// Saves Payment card details
        ///</summary>
        public void SaveUpdateCard(Payment card);

        ///<summary>
        /// Checks address id exist or not
        ///</summary>
        ///<return>bool</return>
        public bool IsAddressIdExist(Guid addressId);

        ///<summary>
        /// Checks Payment id exist or not
        ///</summary>
        ///<return>bool</return>
        public bool IsPaymentIdExist(Guid paymentId);

        ///<summary>
        /// Gets Addres of the user
        ///</summary>
        ///<return>Address</return>
        public Address GetAddress(Guid addressId);

        ///<summary>
        /// Gets Payment details of the user
        ///</summary>
        ///<return>Payment</return>
        public Payment GetPaymentDetails(Guid paymentId);

        ///<summary>
        /// Checks is user details exist or not
        ///</summary>
        ///<return>bool</return>
        public bool IsUserPaymentDetailsExist(Guid cardId,Guid id);
    }
}
