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
        public ErrorDTO IsCardExists(string cardNo,Guid id);

        ///<summary>
        /// Checks whether the card details already exists.
        ///</summary>
        ///<return>Guid</return>
        public Guid SaveCard(CardDTO cardDTO,Guid id);

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
        public void SaveUpadateUser(UpdateUserDTO updateUserDTO);

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


        public ErrorDTO IsUserDetailsAlreadyExist(UpdateUserDTO updateUserDTO);

        public PaymentDetailsResponseDTO GetPaymentDetails(Guid id);

        public ErrorDTO IsCardDetailsExist(UpdateCardDTO updateCardDTO);

        public ErrorDTO CheckUser(Guid id);

        public ErrorDTO CheckUpi(UpiDTO upiDTO);

        public Guid SaveUpi(UpiDTO upiDTO);

        public ErrorDTO IsUpiDetailsExist(UpdateUpiDTO updateUpiDTO);

        public ErrorDTO UpdateUpiDetails(UpdateUpiDTO updateUpiDTO);

        public ErrorDTO IsPaymentDetailsExist(CheckOutCart checkOutDetailsDTO);

        public string GetCheckOutPaymentDetails(CheckOutCart checkOutDetailsDTO);

       // public string GetCheckOutDetails(CheckOutDetailsDTO checkOutDetailsDTO);
       public ErrorDTO IsUserDetailsExist(UpdateCardDTO cardDTO);

        public ErrorDTO IsUserExist(Guid id);

    }
}
