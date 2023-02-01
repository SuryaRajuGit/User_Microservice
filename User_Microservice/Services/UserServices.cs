﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using User_Microservice.Contracts;
using User_Microservice.Entity.Dto;
using User_Microservice.Entity.Models;

namespace User_Microservice.Services
{
    public class UserServices : IUserServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _context;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        public UserServices(IUserRepository userRepository, IMapper mapper, IHttpContextAccessor context)
        {
            _context = context;
            _userRepository = userRepository;
            _mapper = mapper;
            IHostBuilder hostBuilder1 = Host.CreateDefaultBuilder()
                .ConfigureServices(Services =>
                {
                    Services.AddHttpClient(Constants.URL, config =>
            config.BaseAddress = new System.Uri(Constants.Url));
                });
            IHost host1 = hostBuilder1.Build();
            _httpClientFactory = host1.Services.GetRequiredService<IHttpClientFactory>();
        }
        ///<summary>
        /// Checks whether the user email already exists.
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsEmailExists(string emailAddress)
        {
            
            bool isEmailExists = _userRepository.IsEmailExists(emailAddress);
            if(isEmailExists)
            {
                return new ErrorDTO { type = "Email", description = emailAddress + " Email already exists" };
            }
            return null;
        }

        ///<summary>
        /// Validates user entered data.
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO ModelStateInvalid(ModelStateDictionary ModelState)
        {
            return new ErrorDTO
            {
                type = ModelState.Keys.FirstOrDefault(),
                description = ModelState.Values.Select(src => src.Errors.Select(src => src.ErrorMessage).FirstOrDefault()).FirstOrDefault()
            };
        }

        ///<summary>
        /// Checks whether the user phone number already exists.
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsPhoneExists(string phoneNumber)
        {
            bool isPhoneExists = _userRepository.IsPhoneExists(phoneNumber);
            if (isPhoneExists)
            {
                return new ErrorDTO { type = "Phone", description = phoneNumber + " Phone number already exists" };
            }
            return null;
        }

        ///<summary>
        /// Checks whether the user address already exists.
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsAddressExists(AddressDTO Address)
        {
            bool isAddressExists = _userRepository.IsAddressExists(Address);
            if (isAddressExists)
            {
                return new ErrorDTO { type = "Address", description =   "Entered Address already exists" };
            }
            return null;
        }

        ///<summary>
        /// Generates AccessToken 
        ///</summary>
        ///<return>string</return>
        public string AccessToken(string id,string role)
        {
            JwtSecurityTokenHandler tokenhandler = new JwtSecurityTokenHandler();
            byte[] tokenKey = Encoding.UTF8.GetBytes(Constants.Key);

            SecurityTokenDescriptor tokenDeprictor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(
                new Claim[]
                {
                           new Claim(Constants.Role,role),
                           new Claim(Constants.Id,id)
                }
                ),
                Expires = DateTime.UtcNow.AddMinutes(90),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenhandler.CreateToken(tokenDeprictor);
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }

        ///<summary>
        /// Saves user details.
        ///</summary>
        ///<return>Guid</return>
        public async Task<Guid> SaveUser(UserDTO userDTO)
        {
            Guid id = Guid.NewGuid();
            User user = _mapper.Map<User>(userDTO);
            user.Id = id;
            user.Address.Id = Guid.NewGuid();
            user.Address.UserId = id;
            user.Phone.Id = Guid.NewGuid();
            user.Phone.UserId = id;
            user.UserSecret = new UserSecret() {Id=Guid.NewGuid(),UserId=id,Password=userDTO.Password };
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(this.key, this.iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(userDTO.Password);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            string password = Convert.ToBase64String(outputBuffer);
            user.UserSecret = new UserSecret {Password = password,Id=Guid.NewGuid(),UserId=id };

            using HttpClient client = _httpClientFactory.CreateClient(Constants.URL);
            string accessToken = AccessToken(id.ToString(),Constants.User);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.User, accessToken);

            HttpResponseMessage response = client.PostAsync("/api/cart/create",
                                  new StringContent(JsonConvert.SerializeObject(id),
                                  Encoding.UTF8, Constants.ContentType)).Result;
            if(!response.IsSuccessStatusCode)
            {
                throw new Exception("Order service is unavailable");
            }
            string result = await response.Content.ReadAsStringAsync();
            object data = JsonConvert.DeserializeObject(result);
            user.CreatedBy = id;
            Guid responseId = _userRepository.SaveUser(user);
            return responseId;
        }

        ///<summary>
        /// Checks user login details.
        ///</summary>
        ///<return>LoginResponseDTO</return>
        public LoginResponseDTO VerifyLoginDetails(LoginDTO loginDTO)
        {
            Tuple<string, string> isDetailsExists = _userRepository.IsLoginDetailsExists(loginDTO);

            string passwordEncrypt = isDetailsExists.Item2;
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(this.key, this.iv);
            byte[] inputbuffer = Convert.FromBase64String(passwordEncrypt);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            string password = Encoding.Unicode.GetString(outputBuffer);
            if (password != loginDTO.Password)
            {
                return null ;
            }
            string role = isDetailsExists.Item1 == Constants.ADMIN ? Constants.Admin : Constants.User;
            Guid userId = _userRepository.GetUserId(loginDTO.EmailAddress);

            string accessToken = AccessToken(userId.ToString(),role);
            LoginResponseDTO response = new LoginResponseDTO()
            {
                AccessToken = accessToken,
                TokenType = Constants.Bearer
            };
            return response;
        }

        ///<summary>
        /// Checks whether the card details already exists.
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsCardExists(string cardNo)
        {
            Guid id = Guid.Parse(_context.HttpContext.User.Claims.First(i => i.Type == Constants.Id).Value);
            bool isCardExists = _userRepository.IsCardExists(cardNo,id);
            if(isCardExists)
            {
                return new ErrorDTO {type="Card",description="Card already exists" };
            }
            return null;
        }

        ///<summary>
        /// Saves user payment card details.
        ///</summary>
        ///<return>id</return>
        public Guid SaveCard(CardDTO cardDTO)
        {
            Guid id = Guid.Parse(_context.HttpContext.User.Claims.First(i => i.Type == Constants.Id).Value);
            Payment paymentCard = _mapper.Map<Payment>(cardDTO);
            paymentCard.Id = Guid.NewGuid();
            paymentCard.UserId = id;
            paymentCard.CreatedDate = DateTime.Now;
            paymentCard.CreatedBy = id;
            paymentCard.IsActive = true;
            return  _userRepository.SaveCard(paymentCard);
        }

        ///<summary>
        /// checks whether the user email address already exists.
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsUpdateUserEmailExists(string emailAddress, Guid userId)
        {
            bool isUpdateUserEmailExists = _userRepository.IsUpdateUserEmailExists(emailAddress,userId);
            if (!isUpdateUserEmailExists)
            {
                return new ErrorDTO { type = "Email", description = emailAddress + " Email already exists" };
            }
            return null;
        }

        ///<summary>
        /// checks whether the user phone number already exists.
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsUpdateUserPhoneExists(string phoneNumber, Guid userId)
        {
            bool isUpdateUserEmailExists = _userRepository.IsUpdateUserPhoneExists(phoneNumber, userId);
            if (!isUpdateUserEmailExists)
            {
                return new ErrorDTO { type = "Pbone number", description = phoneNumber + " already exists" };
            }
            return null;
        }

        ///<summary>
        /// Checks whether the user already exists.
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsUpdateUserAddressExists(Address address, Guid userId)
        {
            bool isAddressExists = _userRepository.IsUpdateUserAddressExists(address, userId);
            if (!isAddressExists)
            {
                return new ErrorDTO { type = "Address", description = address + " Address  already exists" };
            }
            return null;
        }

        ///<summary>
        /// Checks whether the user exists
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsUserExists(Guid userId)
        {
            bool isUserExist = _userRepository.IsUserExist(userId);
            if(!isUserExist)
            {
                return new ErrorDTO() {type="User",description="User id not found" };
            }
            return null;
        }
        ///<summary>
        /// checks user details exist 
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsUserDetailsExist(UpdateCardDTO cardDTO)
        {
            string userId = _context.HttpContext.User.Claims.First(i => i.Type == Constants.Id).Value;
            ErrorDTO user = IsUserExists(cardDTO.UserId);
            if(user != null)
            {
                return new ErrorDTO() { type = "User", description = "User id not found" };
            }

            bool payment = _userRepository.IsUserPaymentDetailsExist(cardDTO.Id,cardDTO.UserId);
            if(!payment)
            {
                return new ErrorDTO() { type = "Card", description = $"Card with id {cardDTO.Id} not found" };
            }
            return null;
        }
        ///<summary>
        /// Gets user details 
        ///</summary>
        ///<return>UserDetailsResponse</return>
        public UserDetailsResponse GetuserDetails(Guid id)
        {
            User response = _userRepository.GetUserDetails(id);
            if(response == null)
            {
                return null;
            }
            AddressDTO addressDTO = _mapper.Map<AddressDTO>(response.Address);
            PhoneDTO phone = _mapper.Map<PhoneDTO>(response.Phone);
         
            UserDetailsResponse userDetailsResponse = new UserDetailsResponse()
            {
                Id = response.Id,
                FirstName = response.FirstName,
                LastName = response.LastName,
                Address = addressDTO,
                Phone = phone,
                EmailAddress = response.EmailAddress
            };
            return userDetailsResponse;

        }

        ///<summary>
        /// Saves updated user details
        ///</summary>
        public void SaveUpdateUser(UpdateUserDTO updateUserDTO)
        {
            
            if(updateUserDTO.Password != null)
            {
                SymmetricAlgorithm algorithm = DES.Create();
                ICryptoTransform transform = algorithm.CreateEncryptor(this.key, this.iv);
                byte[] inputbuffer = Encoding.Unicode.GetBytes(updateUserDTO.Password);
                byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
                string password = Convert.ToBase64String(outputBuffer);
                updateUserDTO.Password = password;
            }
            User accountInDb = _userRepository.GetUserAccount(updateUserDTO.Id);
            var properties = typeof(UpdateUserDTO).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                object propertyValue = property.GetValue(updateUserDTO);

                if (propertyValue != null)
                {
                    accountInDb.GetType().GetProperty(propertyName).SetValue(accountInDb, propertyValue);
                }
            }

            //accountInDb.FirstName = updateUserDTO.FirstName;
            //accountInDb.LastName = updateUserDTO.LastName;
            //account.EmailAddress = updateUserDTO.EmailAddress;
            //account.Role = null;
            //account.Phone.PhoneNumber = updateUserDTO.Phone.PhoneNumber;
            //account.Phone.Type = updateUserDTO.Phone.Type;
            //account.Address.Line1 = updateUserDTO.Address.Line1;
            //account.Address.Line2 = updateUserDTO.Address.Line2;
            //account.Address.Zipcode = updateUserDTO.Address.Zipcode;
            //account.Address.StateName = updateUserDTO.Address.StateName;
            //account.Address.City = updateUserDTO.Address.City;
            //account.Address.Country = updateUserDTO.Address.Country;
            //account.Address.Type = updateUserDTO.Address.Type;
            //account.UserSecret.Password = password;
            //account.CreatedBy = account.Id;
            accountInDb.UpdatedBy = accountInDb.Id;
            accountInDb.UpdateDate = DateTime.Now;
            _userRepository.SaveUpdateUser(accountInDb);
        }

        ///<summary>
        /// Deletes user account
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO DeleteAccount(Guid id)
        {
            
            using HttpClient client = _httpClientFactory.CreateClient(Constants.URL);
            string accessToken = AccessToken(id.ToString(), Constants.User);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, accessToken);

            HttpResponseMessage response =  client.DeleteAsync($"/api/user/{id}").Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Order service is unavailable");
            }
            bool isAccountDeleted = _userRepository.DeleteUser(id);
            if (!isAccountDeleted)
            {
                return new ErrorDTO() { type = "User", description = "User with id not found" };
            }
            return null;
        }

        ///<summary>
        /// check user details exist
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsUserDetailsAlreadyExist(UpdateUserDTO updateUserDTO)
        {
            if (updateUserDTO.EmailAddress != null)
            {
                bool isUpdateUserEmailExists = _userRepository.IsUpdateUserEmailExists(updateUserDTO.EmailAddress, updateUserDTO.Id);
                if (!isUpdateUserEmailExists)
                {
                    return new ErrorDTO { type = "Email", description = updateUserDTO.EmailAddress + " Email already exists" };
                }
            }
            if(updateUserDTO.Phone != null)
            {
                bool isUpdateUserPhoneExists = _userRepository.IsUpdateUserPhoneExists(updateUserDTO.Phone.PhoneNumber, updateUserDTO.Id);
                if (!isUpdateUserPhoneExists)
                {
                    return new ErrorDTO { type = "Phone number", description = updateUserDTO.Phone.PhoneNumber + "Phone already exists" };
                }
            }
            if(updateUserDTO.Address != null)
            {
                Address address = new Address()
                {
                    Line1 = updateUserDTO.Address.Line1,
                    Line2 = updateUserDTO.Address.Line2,
                    City = updateUserDTO.Address.City,
                    StateName = updateUserDTO.Address.StateName,
                    Country = updateUserDTO.Address.Country,
                    Zipcode = updateUserDTO.Address.Zipcode,
                    Type = updateUserDTO.Address.Type
                };
                bool isAddressExists = _userRepository.IsUpdateUserAddressExists(address, updateUserDTO.Id);
                if (isAddressExists)
                {
                    return new ErrorDTO { type = "Address", description = updateUserDTO.Address + " Address  already exists" };
                }
            }
            return null;
        }
        ///<summary>
        /// Gets user paymant details
        ///</summary>
        ///<return>PaymentDetailsResponseDTO</return>
        public PaymentDetailsResponseDTO GetPaymentDetails(Guid id)
        {
            PaymentDetailsResponseDTO paymentDetailsResponseDTO = new PaymentDetailsResponseDTO()
            {
                Card = new List<CardDTO>(),
                Upi=new List<UpiDTO>()
            };
            List<Payment> paymentDetailsList = _userRepository.GetCardDetails(id); 
            if(paymentDetailsList.Count() == 0)
            {
                return null;
            }
            foreach (Payment item in paymentDetailsList)
            {
                if (item.ExpiryDate == null)
                {
                    UpiDTO upiDTO = new UpiDTO()
                    {
                        Id=item.Id,
                        Name=item.Name,
                        Upi=item.CardNo
                    };
                    paymentDetailsResponseDTO.Upi.Add(upiDTO);
                }
                else
                {
                    CardDTO cardDTO = new CardDTO()
                    {
                        Id = item.Id,
                        CardNo = item.CardNo,
                        ExpiryDate = item.ExpiryDate,
                        Name = item.Name
                    };
                    paymentDetailsResponseDTO.Card.Add(cardDTO);
                }
            }
            return paymentDetailsResponseDTO;
        }

        ///<summary>
        /// Check user card details
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsCardDetailsExist(UpdateCardDTO cardDTO)
        {
            Payment card = _userRepository.GetPaymentDetails(cardDTO.Id);
            card.Id = cardDTO.Id;
            card.UserId = cardDTO.UserId;
            card.ExpiryDate = cardDTO.ExpiryDate;
            card.Name = cardDTO.Name;
            card.CardNo = cardDTO.CardNo;
            
            bool IsCardDetailsExist = _userRepository.isCardDetailsExist(card);
            if(IsCardDetailsExist)
            {
                return new ErrorDTO() {type="Card",description="Card already added" };
            }
            return null;
        }

        ///<summary>
        /// Checks user exist or not
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO CheckUser(Guid id)
        {
            bool isUpiExist = _userRepository.IsUserExist(id);
            if(!isUpiExist)
            {
                return new ErrorDTO() {type="User",description=$"User id not Exist {id}" };
            }
            return null;
        }

        ///<summary>
        /// Checks Upi exist or not
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO CheckUpi(UpiDTO upiDTO, Guid id)
        {
            User user = _userRepository.GetUserAccount(id);
            foreach (Payment item in user.Payment)
            {
                if(item.ExpiryDate == null && item.CardNo == upiDTO.Upi)
                {
                    return new ErrorDTO() { type = "Upi", description = $"Upi {upiDTO.Upi} already added" };
                }
            }
            return null;   
        }

        ///<summary>
        /// Saves user UPI details
        ///</summary>
        ///<return>Guid</return>
        public Guid SaveUpi(UpiDTO upiDTO,Guid id)
        {
            Payment card = new Payment()
            {
                Name = upiDTO.Name,
                CardNo = upiDTO.Upi,
                ExpiryDate = null,
                Id = Guid.NewGuid(),
                UserId = id
            };
            Guid cardId = _userRepository.SaveCard(card);
            return cardId;
        }

        ///<summary>
        /// Checks user details exist or not
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsUpiDetailsExist(UpdateUpiDTO updateUpiDTO)
        {
            string userId = _context.HttpContext.User.Claims.First(i => i.Type == Constants.Id).Value;
            bool isUserExist = _userRepository.IsUserExist(Guid.Parse(userId)); 
            if(isUserExist == false)
            {
                return new ErrorDTO() {type="User",description="User with id not found" };
            }
            bool isUpiExist = _userRepository.CheckUpi(updateUpiDTO.Id);
            if(isUpiExist == false)
            {
                return new ErrorDTO() { type = "Upi", description = "Upi with id not found" };
            }
            return null;
        }

        ///<summary>
        /// Updates user UPI details
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO UpdateUpiDetails(UpdateUpiDTO updateUpiDTO)
        {
            Guid userId = Guid.Parse(_context.HttpContext.User.Claims.First(i => i.Type == Constants.Id).Value);
            updateUpiDTO.UserId = userId;
            List<Payment> paymentList = _userRepository.GetUpiDetails(userId);
            foreach (Payment item in paymentList)
            {
                if(item.CardNo == updateUpiDTO.Upi && item.Id != updateUpiDTO.Id)
                {
                    return new ErrorDTO() {type="Upi",description="Upi already added" };
                }
            }
            Payment card = paymentList.Where(find => find.Id == updateUpiDTO.Id).First();
            card.Name = updateUpiDTO.Name;
            card.CardNo = updateUpiDTO.Upi;
            _userRepository.SaveUpdateCard(card);
            return null;
        }

        ///<summary>
        /// Checks Payment details exist or not
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsPaymentDetailsExist(CheckOutCart checkOutDetailsDTO)
        {
            bool isAddressIdExist = _userRepository.IsAddressIdExist(checkOutDetailsDTO.AddressId);
            if(!isAddressIdExist)
            {
                return new ErrorDTO() {type="Address",description=$"Address with id {checkOutDetailsDTO.AddressId} not exist" };
            }
            bool isPaymentIdExist = _userRepository.IsPaymentIdExist(checkOutDetailsDTO.PaymentId);
            if(!isPaymentIdExist)
            {
                return new ErrorDTO() {type="Payment",description=$"Payment with id {checkOutDetailsDTO.PaymentId} not exist" };
            }
            return null;
        }
        ///<summary>
        /// Checks Address and Payment ids 
        ///</summary>
        ///<return>string</return>
        public string GetCheckOutPaymentDetails(CheckOutCart checkOutDetailsDTO)
        {
            Address address = _userRepository.GetAddress(checkOutDetailsDTO.AddressId);
            Payment payment = _userRepository.GetPaymentDetails(checkOutDetailsDTO.PaymentId);
            CheckOutResponseDTO checkOutResponse = new CheckOutResponseDTO()
            {
                Address = address,
                Payment = payment
            };
            return JsonConvert.SerializeObject(checkOutResponse);

        }
        ///<summary>
        /// Checks User id exist or not
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsUserExist(Guid id)
        {
            ErrorDTO user = IsUserExists(id);
            if (user != null)
            {
                return new ErrorDTO() {type="User",description="User account not found" };
            }
            return null;
        }
    }
}









