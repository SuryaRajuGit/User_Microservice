using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User_Microservice.Entity.Dto;
using User_Microservice.Entity.Models;

namespace User_Microservice.Contracts
{
    public interface IUserServices
    {
        ///<summary>
        /// Checks whether the user email already exists.
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsEmailExists(string emailAddress);

        ///<summary>
        /// Validates user entered data.
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO ModelStateInvalid(ModelStateDictionary modelState);

        ///<summary>
        /// Checks whether the user phone number already exists.
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsPhoneExists(string phoneNumber);

        ///<summary>
        /// Checks whether the user address already exists.
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsAddressExists(AddressDTO address);

        ///<summary>
        /// Saves user details.
        ///</summary>
        ///<return>Guid</return>
        public Task<Guid> SaveUser(UserDTO userDTO);

        ///<summary>
        /// Checks user login details.
        ///</summary>
        ///<return>LoginResponseDTO</return>
        public LoginResponseDTO VerifyLoginDetails(LoginDTO loginDTO);

        ///<summary>
        /// Checks whether the card details already exists.
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsCardExists(string cardNo);

        ///<summary>
        /// Checks whether the card details already exists.
        ///</summary>
        ///<return>Guid</return>
        public Guid SaveCard(CardDTO cardDTO);

        //    public Task<ErrorDTO> IsProductExists(ProductDTO productDTO);

        ///<summary>
        /// Checks whether the card details already exists.
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsUpdateUserEmailExists(string emailAddress,Guid userId);

        ///<summary>
        /// Checks whether the phone number already exists.
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsUpdateUserPhoneExists(string phoneNumber, Guid userId);

        ///<summary>
        /// Checks whether the user address already exists.
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsUpdateUserAddressExists(Address address, Guid userId);

        ///<summary>
        /// Checks whether the phone number already exists.
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsUserExists(Guid userId);

        ///<summary>
        /// Saves user details.
        ///</summary>
        public void SaveUpdateUser(UpdateUserDTO updateUserDTO);

        ///<summary>
        /// Gets user details .
        ///</summary>
        ///<return>UserDetailsResponse</return>
        public UserDetailsResponse GetuserDetails(Guid id);

        ///<summary>
        /// Deletes user account
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO DeleteAccount(Guid id);

        ///<summary>
        /// check user details exist
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsUserDetailsAlreadyExist(UpdateUserDTO updateUserDTO);

        ///<summary>
        /// Check user card details
        ///</summary>
        ///<return>ErrorDTO</return>
        public PaymentDetailsResponseDTO GetPaymentDetails(Guid id);

        ///<summary>
        /// Check user card details
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsCardDetailsExist(UpdateCardDTO updateCardDTO);

        ///<summary>
        /// Checks user exist or not
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO CheckUser(Guid id);

        ///<summary>
        /// Checks Upi exist or not
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO CheckUpi(UpiDTO upiDTO,Guid id);

        ///<summary>
        /// Saves user UPI details
        ///</summary>
        ///<return>Guid</return>
        public Guid SaveUpi(UpiDTO upiDTO, Guid id);

        ///<summary>
        /// Checks user details exist or not
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsUpiDetailsExist(UpdateUpiDTO updateUpiDTO);

        ///<summary>
        /// Updates user UPI details
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO UpdateUpiDetails(UpdateUpiDTO updateUpiDTO);

        ///<summary>
        /// Checks Payment details exist or not
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsPaymentDetailsExist(CheckOutCart checkOutDetailsDTO);

        ///<summary>
        /// Checks Address and Payment ids 
        ///</summary>
        ///<return>string</return>
        public string GetCheckOutPaymentDetails(CheckOutCart checkOutDetailsDTO);

        // public string GetCheckOutDetails(CheckOutDetailsDTO checkOutDetailsDTO);
        ///<summary>
        /// checks user details exist 
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsUserDetailsExist(UpdateCardDTO cardDTO);

        ///<summary>
        /// Checks User id exist or not
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsUserExist(Guid id);

    }
}
