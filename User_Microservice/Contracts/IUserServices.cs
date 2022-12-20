﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User_Microservice.Entity.Dto;

namespace User_Microservice.Contracts
{
    public interface IUserServices
    {
        public ErrorDTO IsEmailExists(string emailAddress);

        public ErrorDTO ModelStateInvalid(ModelStateDictionary modelState);

        public ErrorDTO IsPhoneExists(string phoneNumber);

        public ErrorDTO IsAddressExists(AddressDTO address);

        public Guid SaveUser(UserDTO userDTO);

        public LoginResponseDTO VerifyLoginDetails(LoginDTO loginDTO);

        public ErrorDTO IsCardExists(string cardNo);

        public Guid SaveCard(CardDTO cardDTO);

    //    public Task<ErrorDTO> IsProductExists(ProductDTO productDTO);

    }
}