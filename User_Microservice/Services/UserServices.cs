using AutoMapper;
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
                return new ErrorDTO { Type = "Conflict", Message = emailAddress + " Email already exists" ,StatusCode="409"};
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
                Type = "BadRequest",
                Message = ModelState.Values.Select(src => src.Errors.Select(src => src.ErrorMessage).FirstOrDefault()).FirstOrDefault(),
                StatusCode="400"
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
                return new ErrorDTO { Type = "Conflict", Message = phoneNumber + " Phone number already exists",StatusCode="409" };
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
                return new ErrorDTO { Type = "Conflict", Message =   "Entered Address already exists",StatusCode="409" };
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
            user.IsActive = true;
            user.CreatedDate = DateTime.Now;
            user.UserSecret = new UserSecret() {Id=Guid.NewGuid(),UserId=id,Password=userDTO.Password,IsActive=true };
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
            user.Address.IsActive = true;
            user.Phone.IsActive = true;
            user.Phone.CreatedDate = DateTime.Now;
            user.Address.CreatedDate = DateTime.Now;
            user.UserSecret.IsActive = true;
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
                Jwt = accessToken,
                Type = Constants.Bearer
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
                return new ErrorDTO {Type="Conflict",Message="Card already exists",StatusCode="409" };
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
                return new ErrorDTO { Type = "Conflict", Message = emailAddress + " Email already exists",StatusCode="409" };
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
                return new ErrorDTO { Type = "Conflict", Message = phoneNumber + " already exists",StatusCode="409" };
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
                return new ErrorDTO { Type = "Conflict", Message = address + " Address  already exists",StatusCode="409" };
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
                return new ErrorDTO() {Type="NotFound",Message="User id not found",StatusCode="404" };
            }
            return null;
        }
        ///<summary>
        /// checks user details exist 
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsUserDetailsExist(Guid id)
        {
            Guid userId = Guid.Parse(_context.HttpContext.User.Claims.First(i => i.Type == Constants.Id).Value);
            bool payment = _userRepository.IsUserPaymentDetailsExist(id, userId);
            if(!payment)
            {
                return new ErrorDTO() { Type = "NotFound", Message = $"Card with id {id} not found",StatusCode="404" };
            }
            return null;
        }
        ///<summary>
        /// Gets user details 
        ///</summary>
        ///<return>UserDetailsResponse</return>
        public UserDetailsResponse GetuserDetails()
        {
            Guid id = Guid.Parse(_context.HttpContext.User.Claims.First(i => i.Type == Constants.Id).Value);
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
            Guid userId = Guid.Parse(_context.HttpContext.User.Claims.First(i => i.Type == Constants.Id).Value);
            updateUserDTO.Id = userId;
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
            PropertyInfo[] properties = typeof(UpdateUserDTO).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                object propertyValue = property.GetValue(updateUserDTO);

                if (propertyValue != null && propertyName != "Address" && propertyName != "Phone" )
                {
                    accountInDb.GetType().GetProperty(propertyName).SetValue(accountInDb, propertyValue);
                }
            }
            
            if (updateUserDTO.Address != null) {
                PropertyInfo[] propertiesAddress = typeof(UpdateAddressDTO).GetProperties();
                foreach (PropertyInfo propertyAddress in propertiesAddress)
                {
                    string propertyName = propertyAddress.Name;
                    object propertyValue = propertyAddress.GetValue(updateUserDTO.Address);

                    if (propertyValue != null)
                    {
                        accountInDb.Address.GetType().GetProperty(propertyName).SetValue(accountInDb.Address, propertyValue);
                    }
                }
                accountInDb.Address.IsActive = true;
            }

            if (updateUserDTO.Phone != null) {
                PropertyInfo[] propertiesPhone = typeof(UpdatePhoneDTO).GetProperties();
                foreach (PropertyInfo propertyPhone in propertiesPhone)
                {
                    string propertyName = propertyPhone.Name;
                    object propertyValue = propertyPhone.GetValue(updateUserDTO.Phone);

                    if (propertyValue != null)
                    {
                        accountInDb.Phone.GetType().GetProperty(propertyName).SetValue(accountInDb.Phone, propertyValue);
                    }
                }
                accountInDb.Phone.IsActive = true;
            }
            accountInDb.UpdatedBy = accountInDb.Id;
            accountInDb.UpdateDate = DateTime.Now;
            _userRepository.SaveUpdateUser(accountInDb);
        }

        ///<summary>
        /// Deletes user account
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO DeleteAccount()
        {
            Guid id = Guid.Parse(_context.HttpContext.User.Claims.First(i => i.Type == Constants.Id).Value);
            using HttpClient client = _httpClientFactory.CreateClient(Constants.URL);
            string accessToken = AccessToken(id.ToString(), Constants.User);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, accessToken);
            //api call to user service
            HttpResponseMessage response =  client.DeleteAsync($"/api/user/{id}").Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Order service is unavailable");
            }
            bool isAccountDeleted = _userRepository.DeleteUser(id);
            if (!isAccountDeleted)
            {
                return new ErrorDTO() { Type = "NotFound", Message = "User with id not found",StatusCode="404" };
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
                    return new ErrorDTO { Type = "Conflict", Message = updateUserDTO.EmailAddress + " Email already exists",StatusCode="409" };
                }
            }
            if(updateUserDTO.Phone != null)
            {
                bool isUpdateUserPhoneExists = _userRepository.IsUpdateUserPhoneExists(updateUserDTO.Phone.PhoneNumber, updateUserDTO.Id);
                if (!isUpdateUserPhoneExists)
                {
                    return new ErrorDTO { Type = "Conflict", Message = updateUserDTO.Phone.PhoneNumber + "Phone already exists",StatusCode="409" };
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
                    return new ErrorDTO { Type = "Conflict", Message = updateUserDTO.Address + " Address  already exists",StatusCode="409" };
                }
            }
            return null;
        }
        ///<summary>
        /// Gets user paymant details
        ///</summary>
        ///<return>PaymentDetailsResponseDTO</return>
        public PaymentDetailsResponseDTO GetPaymentDetails()
        {
            Guid id = Guid.Parse(_context.HttpContext.User.Claims.First(i => i.Type == Constants.Id).Value);
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
        public ErrorDTO IsCardDetailsExist(UpdateCardDTO cardDTO,Guid id)
        {
            Payment card = _userRepository.GetPaymentDetails(id);
            card.ExpiryDate = cardDTO.ExpiryDate == null ? card.ExpiryDate : cardDTO.ExpiryDate;
            card.Name = cardDTO.Name == null ? card.Name : cardDTO.Name;
            card.CardNo = cardDTO.CardNo == null ? card.CardNo : cardDTO.CardNo ;
            card.UpdateDate = DateTime.Now;
            card.IsActive = true;
            bool IsCardDetailsExist = _userRepository.IsCardDetailsExist(card);
            if(IsCardDetailsExist)
            {
                return new ErrorDTO() {Type="Conflict",Message="Card already added",StatusCode="409" };
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
                return new ErrorDTO() {Type="User",Message=$"User id not Exist {id}",StatusCode="404" };
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
                    return new ErrorDTO() { Type = "Conflict", Message = $"Upi {upiDTO.Upi} already added",StatusCode="409" };
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
                UserId = id,
                IsActive = true,
                CreatedDate = DateTime.Now,
                UpdatedBy = id
            };
            Guid cardId = _userRepository.SaveCard(card);
            return cardId;
        }

        ///<summary>
        /// Checks user details exist or not
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsUpiDetailsExist(UpdateUpiDTO updateUpiDTO,Guid id)
        {
            string userId = _context.HttpContext.User.Claims.First(i => i.Type == Constants.Id).Value;
            bool isUserExist = _userRepository.IsUserExist(Guid.Parse(userId)); 
            if(isUserExist == false)
            {
                return new ErrorDTO() {Type="NotFound",Message="User with id not found",StatusCode="404" };
            }
            bool isUpiExist = _userRepository.CheckUpi(id);
            if(isUpiExist == false)
            {
                return new ErrorDTO() { Type = "Upi", Message = "Upi with id not found",StatusCode="404" };
            }
            return null;
        }

        ///<summary>
        /// Updates user UPI details
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO UpdateUpiDetails(UpdateUpiDTO updateUpiDTO,Guid id)
        {
            Guid userId = Guid.Parse(_context.HttpContext.User.Claims.First(i => i.Type == Constants.Id).Value);
            
            List<Payment> paymentList = _userRepository.GetUpiDetails(userId);
            foreach (Payment item in paymentList)
            {
                if(item.CardNo == updateUpiDTO.Upi && item.Id != id)
                {
                    return new ErrorDTO() {Type="Conflict",Message="Upi already added",StatusCode="409" };
                }
            }
            Payment card = paymentList.Where(find => find.Id == id).First();
            PropertyInfo[] properties = typeof(UpdateUpiDTO).GetProperties();
            card.CardNo = updateUpiDTO.Upi == null ? card.CardNo : updateUpiDTO.Upi;
            card.Name = updateUpiDTO.Name == null ? card.Name : updateUpiDTO.Name;
            card.UpdateDate = DateTime.Now;
            card.UpdatedBy = userId;
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
                return new ErrorDTO() {Type="NotFound",Message=$"Address with id {checkOutDetailsDTO.AddressId} not exist",StatusCode="404" };
            }
            bool isPaymentIdExist = _userRepository.IsPaymentIdExist(checkOutDetailsDTO.PaymentId);
            if(!isPaymentIdExist)
            {
                return new ErrorDTO() {Type= "NotFound", Message=$"Payment with id {checkOutDetailsDTO.PaymentId} not exist",StatusCode="404" };
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
                return new ErrorDTO() {Type="NotFound",Message="User account not found",StatusCode="404" };
            }
            return null;
        }
    }
}









