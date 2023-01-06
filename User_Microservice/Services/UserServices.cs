using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;

        byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        public UserServices(IUserRepository userRepository,IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
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
            byte[] tokenKey = Encoding.UTF8.GetBytes("thisismySecureKey12345678");

            SecurityTokenDescriptor tokenDeprictor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(
                new Claim[]
                {
                           new Claim("role",role),
                           new Claim("Id",id)
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
            //User user = new User();
            //userDTO.UserSecret = new UserSecretDTO { Password = userDTO.Password };
            Guid id = Guid.NewGuid();
            //user.Id = Id;
            //user.FirstName = userDTO.FirstName;
            //user.LastName = userDTO.LastName;
            //user.Phone = new Phone { PhoneNumber = userDTO.Phone.PhoneNumber, Id = Guid.NewGuid(), UserId = Id,Type=userDTO.Phone.Type };
            //user.EmailAddress = userDTO.EmailAddress;
            //user.Address = new Address {
            //    Line1= userDTO.Address.Line1,
            //    Line2 = userDTO.Address.Line2,
            //    Zipcode= userDTO.Address.Zipcode,
            //    StateName=userDTO.Address.StateName,
            //    City=userDTO.Address.City,
            //    Country=userDTO.Address.Country,
            //    Type=userDTO.Address.Type
            //    };
            
            
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

            using HttpClient client = _httpClientFactory.CreateClient("cart");
            string accessToken = AccessToken(id.ToString(),"User");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = client.PostAsync("/api/cart/create",
                                  new StringContent(JsonConvert.SerializeObject(id),
                                  Encoding.UTF8, "application/json")).Result;
            if(!response.IsSuccessStatusCode)
            {
                throw new Exception("Order service is unavailable");
            }
            string result = await response.Content.ReadAsStringAsync();
            object data = JsonConvert.DeserializeObject(result);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

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

            string text = isDetailsExists.Item2;
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(this.key, this.iv);
            byte[] inputbuffer = Convert.FromBase64String(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            string password = Encoding.Unicode.GetString(outputBuffer);

            if (password != loginDTO.Password)
            {
                return null ;
            }
            string role = isDetailsExists.Item1 == "ADMIN" ? "Admin" : "User";
            Guid userId = _userRepository.GetUserId(loginDTO.EmailAddress);
            //JwtSecurityTokenHandler tokenhandler = new JwtSecurityTokenHandler();
            //byte[] tokenKey = Encoding.UTF8.GetBytes("thisismySecureKey12345678");
            //Guid userId = _userRepository.GetUserId(loginDTO.EmailAddress);
            //SecurityTokenDescriptor tokenDeprictor = new SecurityTokenDescriptor
            //{
            //    Subject = new System.Security.Claims.ClaimsIdentity(
            //        new Claim[]
            //        {
            //               new Claim("role",role),
            //               new Claim("Id",userId.ToString())
            //        }
            //        ),
            //    Expires = DateTime.UtcNow.AddMinutes(90),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            //};

            //SecurityToken token = tokenhandler.CreateToken(tokenDeprictor);
            //string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            string accessToken = AccessToken(userId.ToString(),role);
            LoginResponseDTO response = new LoginResponseDTO()
            {
                AccessToken = accessToken,
                TokenType = "Bearer"
            };
            return response;
        }

        ///<summary>
        /// Checks whether the card details already exists.
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO IsCardExists(string cardNo,Guid id)
        {
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
        public Guid SaveCard(CardDTO cardDTO,Guid id)
        {
            //Payment card = new Payment();
            //card.Id = Guid.NewGuid();
            //card.Name = cardDTO.Name;
            //card.CardNo = cardDTO.CardNo;
            //card.UserId = id;
            //card.ExpiryDate = cardDTO.ExpiryDate;
            Payment paymentCard = _mapper.Map<Payment>(cardDTO);
            paymentCard.Id = Guid.NewGuid();
            paymentCard.UserId = id;
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
        public ErrorDTO IsUserDetailsExist(UpdateCardDTO cardDTO)
        {
            var x = IsUserExists(cardDTO.UserId);
            if(x != null)
            {
                return new ErrorDTO() { type = "User", description = "User id not found" };
            }

            var xx = _userRepository.IsUserPaymentDetailsExist(cardDTO.Id,cardDTO.UserId);
            if(!xx)
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
            AddressDTO address = new AddressDTO()
            {
                Line1 = response.Address.Line1,
                Line2 = response.Address.Line2,
                StateName = response.Address.StateName,
                City = response.Address.City,
                Zipcode = response.Address.Zipcode,
                Country = response.Address.Country,
                Type = response.Address.Type
            };

            //CardDTO card = new CardDTO();
            //card.CardNo = response.Card.CardNo;
            //card.ExpiryDate = response.Card.ExpiryDate;
            //card.Name = response.Card.CardHolderName;


            PhoneDTO phoneDTO = new PhoneDTO()
            {
                PhoneNumber = response.Phone.PhoneNumber,
                Type = response.Phone.Type,
            };
            UserDetailsResponse userDetailsResponse = new UserDetailsResponse()
            {
                Id = response.Id,
                FirstName = response.FirstName,
                LastName = response.LastName,
                Address = address,
               // Card = card,
                Phone = phoneDTO,
                EmailAddress = response.EmailAddress
            };
            return userDetailsResponse;

        }

        ///<summary>
        /// Saves updated user details
        ///</summary>
        public void SaveUpadateUser(UpdateUserDTO updateUserDTO)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(this.key, this.iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(updateUserDTO.Password);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            string password = Convert.ToBase64String(outputBuffer);

            User account = _userRepository.GetUserAccount(updateUserDTO.Id);
            account.FirstName = updateUserDTO.FirstName;
            account.LastName = updateUserDTO.LastName;
            account.EmailAddress = updateUserDTO.EmailAddress;
            account.Role = null;
            account.Phone.PhoneNumber = updateUserDTO.Phone.PhoneNumber;
            account.Phone.Type = updateUserDTO.Phone.Type;
            account.Address.Line1 = updateUserDTO.Address.Line1;
            account.Address.Line2 = updateUserDTO.Address.Line2;
            account.Address.Zipcode = updateUserDTO.Address.Zipcode;
            account.Address.StateName = updateUserDTO.Address.StateName;
            account.Address.City = updateUserDTO.Address.City;
            account.Address.Country = updateUserDTO.Address.Country;
            account.Address.Type = updateUserDTO.Address.Type;
            account.UserSecret.Password = password;
            _userRepository.SaveUpdateUser(account);
        }

        ///<summary>
        /// Deletes user account
        ///</summary>
        ///<return>ErrorDTO</return>
        public ErrorDTO DeleteAccount(Guid id)
        {
            
            using HttpClient client = _httpClientFactory.CreateClient("cart");
            string accessToken = AccessToken(id.ToString(), "User");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response =  client.DeleteAsync($"/api/user/{id}").Result;
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
        public ErrorDTO IsUserDetailsAlreadyExist(UpdateUserDTO updateUserDTO)
        {
            bool isUserExist = _userRepository.IsUserExist(updateUserDTO.Id);
            if (!isUserExist)
            {
                return new ErrorDTO() { type = "User", description = "User id not found" };
            }
            bool isUpdateUserEmailExists = _userRepository.IsUpdateUserEmailExists(updateUserDTO.EmailAddress, updateUserDTO.Id);
            if (!isUpdateUserEmailExists)
            {
                return new ErrorDTO { type = "Email", description = updateUserDTO.EmailAddress + " Email already exists" };
            }
            bool isUpdateUserPhoneExists = _userRepository.IsUpdateUserPhoneExists(updateUserDTO.Phone.PhoneNumber, updateUserDTO.Id);
            if (!isUpdateUserPhoneExists)
            {
                return new ErrorDTO { type = "Pbone number", description = updateUserDTO.Phone.PhoneNumber + "Phone already exists" };
            }
            Address address = new Address()
            {
                Line1 =updateUserDTO.Address.Line1,
                Line2=updateUserDTO.Address.Line2,
                City=updateUserDTO.Address.City,
                StateName=updateUserDTO.Address.StateName,
                Country=updateUserDTO.Address.Country,
                Zipcode=updateUserDTO.Address.Zipcode,
                Type=updateUserDTO.Address.Type
            };
            bool isAddressExists = _userRepository.IsUpdateUserAddressExists(address, updateUserDTO.Id);
            if (isAddressExists)
            {
                return new ErrorDTO { type = "Address", description = updateUserDTO.Address + " Address  already exists" };
            }
            
            return null;
        }
        public PaymentDetailsResponseDTO GetPaymentDetails(Guid id)
        {
            PaymentDetailsResponseDTO paymentDetailsResponseDTO = new PaymentDetailsResponseDTO()
            {
                Card = new List<CardDTO>(),
                Upi=new List<UpiDTO>()
            };
            List<Payment> x = _userRepository.GetCardDetails(id); 
            if(x.Count() == 0)
            {
                return null;
            }
            foreach (var item in x)
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
        public ErrorDTO CheckUser(Guid id)
        {
            bool x = _userRepository.CheckUpi(id);
            if(!x)
            {
                return new ErrorDTO() {type="User",description=$"User id not Exist {id}" };
            }
            return null;
        }
        public ErrorDTO CheckUpi(UpiDTO upiDTO)
        {
            User x = _userRepository.GetUserAccount(upiDTO.Id);
            foreach (var item in x.Payment)
            {
                if(item.ExpiryDate == null && item.CardNo == upiDTO.Upi)
                {
                    return new ErrorDTO() { type = "Upi", description = $"Upi {upiDTO.Upi} already added" };
                }
            }


        
            return null;   
        }
        public Guid SaveUpi(UpiDTO upiDTO)
        {
            Payment card = new Payment()
            {
                Name = upiDTO.Name,
                CardNo = upiDTO.Upi,
                ExpiryDate = null,
                Id = Guid.NewGuid(),
                UserId = upiDTO.Id
            };
            var id = _userRepository.SaveCard(card);
            return id;
        }
        public ErrorDTO IsUpiDetailsExist(UpdateUpiDTO updateUpiDTO)
        {
            var g = _userRepository.IsUserExist(updateUpiDTO.UserId); 
            if(g == false)
            {
                return new ErrorDTO() {type="User",description="User with id not found" };
            }
            var t = _userRepository.CheckUpi(updateUpiDTO.Id);
            if(t == false)
            {
                return new ErrorDTO() { type = "Upi", description = "Upi with id not found" };
            }
            return null;
        }
        public ErrorDTO UpdateUpiDetails(UpdateUpiDTO updateUpiDTO)
        {
            List<Payment> g = _userRepository.GetUpiDetails(updateUpiDTO.UserId);

            foreach (var item in g)
            {
                if(item.CardNo == updateUpiDTO.Upi && item.Id != updateUpiDTO.Id)
                {
                    return new ErrorDTO() {type="Upi",description="Upi already added" };
                }
            }
            Payment card = g.Where(find => find.Id == updateUpiDTO.Id).First();
            card.Name = updateUpiDTO.Name;
            card.CardNo = updateUpiDTO.Upi;
            _userRepository.SaveUpdateCard(card);
            return null;
        }
        public ErrorDTO IsPaymentDetailsExist(CheckOutCart checkOutDetailsDTO)
        {
            var g = _userRepository.IsAddressIdExist(checkOutDetailsDTO.AddressId);
            if(!g)
            {
                return new ErrorDTO() {type="Address",description=$"Address with id {checkOutDetailsDTO.AddressId} not exist" };
            }
            var t = _userRepository.IsPaymentIdExist(checkOutDetailsDTO.PaymentId);
            if(!t)
            {
                return new ErrorDTO() {type="Payment",description=$"Payment with id {checkOutDetailsDTO.PaymentId} not exist" };
            }
            return null;
        }
        public string GetCheckOutPaymentDetails(CheckOutCart checkOutDetailsDTO)
        {
            var g = _userRepository.GetAddress(checkOutDetailsDTO.AddressId);
            var p = _userRepository.GetPaymentDetails(checkOutDetailsDTO.PaymentId);
            CheckOutResponseDTO checkOutResponse = new CheckOutResponseDTO()
            {
                Address = g,
                Payment = p
            };
            return JsonConvert.SerializeObject(checkOutResponse);

        }
        public ErrorDTO IsUserExist(Guid id)
        {
            var x = IsUserExist(id);
            if (x != null)
            {
                return new ErrorDTO() {type="User",description="User accound deleted" };
            }
            return null;
        }
    }
}
