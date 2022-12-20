using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
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
        public UserServices(IUserRepository userRepository,IMapper _mapper, IHttpClientFactory httpClientFactory)
        {
            _userRepository = userRepository;
            _mapper = _mapper;
            _httpClientFactory = httpClientFactory;
        }

        public ErrorDTO IsEmailExists(string emailAddress)
        {
            
            bool isEmailExists = _userRepository.IsEmailExists(emailAddress);
            if(isEmailExists)
            {
                return new ErrorDTO { type = "Email", description = emailAddress + " Email already exists" };
            }
            return null;
        }

        public ErrorDTO ModelStateInvalid(ModelStateDictionary ModelState)
        {
            return new ErrorDTO
            {
                type = ModelState.Keys.FirstOrDefault(),
                description = ModelState.Values.Select(src => src.Errors.Select(src => src.ErrorMessage).FirstOrDefault()).FirstOrDefault()
            };
        }

        public ErrorDTO IsPhoneExists(string phoneNumber)
        {
            bool isPhoneExists = _userRepository.IsPhoneExists(phoneNumber);
            if (isPhoneExists)
            {
                return new ErrorDTO { type = "Phone", description = phoneNumber + " Phone number already exists" };
            }
            return null;
        }

        public ErrorDTO IsAddressExists(AddressDTO Address)
        {
            bool isAddressExists = _userRepository.IsAddressExists(Address);
            if (isAddressExists)
            {
                return new ErrorDTO { type = "Address", description = Address + " Address  already exists" };
            }
            return null;
        }

        public Guid SaveUser(UserDTO userDTO)
        {
            User x = new User();
            userDTO.UserSecret = new UserSecretDTO { Password = userDTO.Password };
            Guid Id = Guid.NewGuid();
            x.FirstName = userDTO.FirstName;
            x.LastName = userDTO.LastName;
            x.Phone = new Phone { PhoneNumber = userDTO.Phone.PhoneNumber, Id = Guid.NewGuid(), UserId = Id };
            x.EmailAddress = userDTO.EmailAddress;
            x.Address = new Address {
                Line1= userDTO.Address.Line1,
                Line2 = userDTO.Address.Line2,
                Zipcode= userDTO.Address.Zipcode,
                StateName=userDTO.Address.StateName,
                City=userDTO.Address.City,
                Country=userDTO.Address.Country,
                Type=userDTO.Address.Type
                };

       //     User user = _mapper.Map<User>(userDTO);
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(this.key, this.iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(userDTO.Password);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            string password = Convert.ToBase64String(outputBuffer);
            x.UserSecret = new UserSecret {Password = password,Id=Guid.NewGuid(),UserId=Id };
            Guid response = _userRepository.SaveUser(x);
            return response;
        }

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

            JwtSecurityTokenHandler tokenhandler = new JwtSecurityTokenHandler();
            byte[] tokenKey = Encoding.UTF8.GetBytes("thisismySecureKey12345678");
            Guid userId = _userRepository.GetUserId(loginDTO.EmailAddress);
            SecurityTokenDescriptor tokenDeprictor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(
                    new Claim[]
                    {
                           new Claim("role",role),
                           new Claim("Id",userId.ToString())
                    }
                    ),
                Expires = DateTime.UtcNow.AddMinutes(90),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenhandler.CreateToken(tokenDeprictor);
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            LoginResponseDTO response = new LoginResponseDTO()
            {
                AccessToken = tokenString,
                TokenType = "Bearer"
            };
            return response;
        }

        public ErrorDTO IsCardExists(string cardNo)
        {
            bool isCardExists = _userRepository.IsCardExists(cardNo);
            if(isCardExists)
            {
                return new ErrorDTO {type="Conflict",description="Card already exists" };
            }
            return null;
        }

        public Guid SaveCard(CardDTO cardDTO)
        {
            Card card = new Card();
            card.Id = Guid.NewGuid();
            card.CardHolderName = cardDTO.CardHolderName;
            card.CardNo = cardDTO.CardNo;
            card.UserId = cardDTO.Id;
            return  _userRepository.SaveCard(card);
        }
        
        //public async Task<ErrorDTO> IsProductExists(ProductDTO productDTO)
        //{
        //    //using var client = _httpClientFactory.CreateClient("product");
        //    //HttpContent content = new StringContent(JsonConvert.SerializeObject(productDTO));
        //    //var response = await client.PostAsync("api/product/ocelot", content);
        //    //var data = await response.Content.

        //}
    }
}
