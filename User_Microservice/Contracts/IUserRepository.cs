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

        public bool IsCardExists(string cardNo,Guid id);

        public Guid SaveCard(Payment cardDTO);

        public bool IsUpdateUserEmailExists(string emailAddress,Guid userId);

        public bool IsUpdateUserPhoneExists(string phoneNumber,Guid userId);

        public bool IsUpdateUserAddressExists(Address address,Guid userId);

        public bool IsUserExist(Guid userId);

        public User GetUserDetails(Guid id);

        public void SaveUpdateUser(User user);

        public bool DeleteUser(Guid id);

        public List<Payment> GetCardDetails(Guid id);

        public bool isCardDetailsExist(Payment card);

        public User GetUserAccount(Guid id);

        public bool CheckUpi(Guid id);

        public List<Payment> GetUpiDetails(Guid userId);

        public void SaveUpdateCard(Payment card);

        public bool IsAddressIdExist(Guid addressId);

        public bool IsPaymentIdExist(Guid paymentId);

        public Address GetAddress(Guid addressId);

        public Payment GetPaymentDetails(Guid paymentId);

        public bool IsUserPaymentDetailsExist(Guid cardId,Guid id);
    }
}
