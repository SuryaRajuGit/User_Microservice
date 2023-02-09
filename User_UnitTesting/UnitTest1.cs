using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using User_Microservice.Controllers;
using User_Microservice.Entity.Dto;
using User_Microservice.Entity.Models;
using User_Microservice.Helpers;
using User_Microservice.Repository;
using User_Microservice.Services;
using Xunit;

namespace User_UnitTesting
{
    public class UnitTest1 
    {
        private readonly IMapper _mapper;
        private readonly UserController _userController;
        private readonly ILogger _logger;
        private UserContext _context;
        private UserRepository _repository;
        private UserServices _userService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public HttpClient CreateClient(string name)
        {
            return _httpClientFactory.CreateClient(name);
        }

        public UnitTest1()
        {

            IHostBuilder hostBuilder = Host.CreateDefaultBuilder().
            ConfigureLogging((builderContext, loggingBuilder) =>
            {
                loggingBuilder.AddConsole((options) =>
                {
                    options.IncludeScopes = true; 
                });
            });
            IHost host = hostBuilder.Build();
            ILogger<UserController> _logger = host.Services.GetRequiredService<ILogger<UserController>>();

            MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Mappers());
            });
            

            Claim claim = new Claim("role", "User");
            Claim claim1 = new Claim("Id", "8d0c1df7-a887-4453-8af3-799e4a7ed013");
            ClaimsIdentity identity = new ClaimsIdentity(new[] { claim, claim1 }, "BasicAuthentication"); 
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            GenericIdentity identityy = new GenericIdentity("some name", "test");
            ClaimsPrincipal contextUser = new ClaimsPrincipal(identity);

            DefaultHttpContext httpContext = new DefaultHttpContext()
            {
                User = contextUser
        };

            HttpContextAccessor _httpContextAccessor = new HttpContextAccessor()
            {
                HttpContext = httpContext
            };
       

            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
            DbContextOptions<UserContext> options = new DbContextOptionsBuilder<UserContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new UserContext(options);
           
            _repository = new UserRepository(_context);
            
            _userService = new UserServices(_repository, _mapper, _httpContextAccessor);

            _userController = new UserController(_userService, _logger);
            
            
            AddData();
            _context.Database.EnsureCreated();
        }
        public void AddData()
        {
            string path = @"C:\Users\Hp\source\repos\User_Microservice\User_Microservice\Entity\UnitTestFiles\UserData.csv";
            string ReadCSV = File.ReadAllText(path);
            List<User> users = new List<User>();
            string[] data = ReadCSV.Split('\r');
            foreach (string item in data)
            {
                string[] row = item.Split(",");
                Guid Id = Guid.Parse(row[19]);
                UserSecret userSecret = new UserSecret { Id = Guid.NewGuid(), UserId = Id, Password = row[0],IsActive=true };
                User user = new User { Role = row[1], FirstName = row[2], LastName = row[3], EmailAddress = row[4], Id = Id 
                ,Address =new Address(),
                Payment =new List<Payment>(),
                Phone = new Phone(),
                UserSecret=new UserSecret(),
                IsActive=true
                };
                Phone phone = new Phone { Id = Guid.NewGuid(),IsActive=true, PhoneNumber = row[5], Type = row[6], UserId = Id };
                Address address = new Address { Id =Guid.NewGuid(),IsActive=true , Line1 = row[7], Line2 = row[8], City = row[9], Zipcode = row[10], StateName = row[11], Country = row[12], Type = row[13], UserId = Id };
                Payment payment = new Payment {Id=Guid.Parse(row[20]),UserId=Id,Name=row[14],CardNo=row[15],ExpiryDate=row[16],IsActive=true };
                Payment payment1 = new Payment {Id=Guid.Parse(row[21]),UserId=Id,Name=row[17],CardNo=row[18],ExpiryDate=null,IsActive=true };

                user.Address = address;
                user.Phone = phone;
                user.UserSecret = userSecret;
                user.Payment.Add(payment);
                user.Payment.Add(payment1);
                user.IsActive = true;
                users.Add(user);   
            }
            _context.AddRange(users);
            _context.SaveChanges();
        }


        [Fact]
        public void VerifyUser_Test()
        {
            LoginDTO inValidlogin = new LoginDTO() {EmailAddress = "user@gmail.com",Password = "jjjjj"};
            LoginDTO validlogin = new LoginDTO() { EmailAddress = "user@gmail.com", Password = "Admin@123" };
            IActionResult unAuthorised = _userController.VerifyUser(inValidlogin);
            IActionResult Authorised = _userController.VerifyUser(validlogin);

            ObjectResult response = Assert.IsType<ObjectResult>(unAuthorised);

            Assert.Equal(401, response.StatusCode);
            Assert.IsType<OkObjectResult>(Authorised);

        }

        [Fact]
        public  async Task SignUp_Test()
        {
            UserDTO user = new UserDTO()
            {
                Password="User1@23",
                FirstName = "sury",
                LastName = "raj",
                Address = new AddressDTO()
                {
                    Line1 = "l",
                    Line2 = "ll",
                    City = "cit",
                    Country = "cou",
                    StateName = "state",
                    Zipcode = "45342",
                    Type = "woek"
                },
                EmailAddress = "surya@gmail.com",
                Phone  = new PhoneDTO()
                {  
                    PhoneNumber = "8142355769",
                    Type = "home",
                },
            };
            IActionResult result = await _userController.SignUp(user);
            IActionResult result1 = await _userController.SignUp(user);

            ObjectResult response = Assert.IsType<ObjectResult>(result);
            ObjectResult response1 = Assert.IsType<ObjectResult>(result1);

            Assert.Equal(201, response.StatusCode);
            Assert.Equal(409, response1.StatusCode);
        }

        [Fact]
        public void UpdateUser_Test()
        {
            UpdateUserDTO updateUserDTO = new UpdateUserDTO()
            {
                Id = Guid.Parse("8d0c1df7-a887-4453-8af3-799e4a7ed013"),
                FirstName = "sury",
                LastName = "raj",
                Address = new UpdateAddressDTO()
                {
                    Line1 = "l",
                    Line2 = "ll",
                    City = "cit",
                    Country = "cou",
                    StateName = "state",
                    Zipcode = "45342",
                    Type = "woek"
                },
                EmailAddress = "surya@gmail.com",
                Phone = new UpdatePhoneDTO()
                {
                    PhoneNumber = "8142355769",
                    Type = "home",
                },
            };
            UpdateUserDTO updateUserDTO1 = new UpdateUserDTO()
            {
                Id = Guid.Parse("8d0c1df7-a887-4453-8af3-799e4a7ed013"),
                FirstName = "sury",
                LastName = "raj",
                Address = new UpdateAddressDTO()
                {
                    Line1 = "l",
                    Line2 = "ll",
                    City = "cit",
                    Country = "cou",
                    StateName = "state",
                    Zipcode = "45342",
                    Type = "woek"
                },
                EmailAddress = "admin@gmail.com",
                Phone = new UpdatePhoneDTO()
                {
                    PhoneNumber = "8142355769",
                    Type = "home",
                },
            };
            IActionResult result =  _userController.UpdateUser(updateUserDTO);
            IActionResult result1 =  _userController.UpdateUser(updateUserDTO1);

            OkObjectResult response = Assert.IsType<OkObjectResult>(result);
            ObjectResult response1 = Assert.IsType<ObjectResult>(result1);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal(409, response1.StatusCode);
            
        }
        [Fact]
        public void AddPaymentCard_Test()
        {
            CardDTO cardDTO = new CardDTO()
            {
                CardNo = "896757876878",
                ExpiryDate = "01/23",
                Id = Guid.NewGuid(),
                Name = "Sury"
            };
            IActionResult response = _userController.AddPaymentCard(cardDTO);
            IActionResult response1 = _userController.AddPaymentCard(cardDTO);

            ObjectResult result = Assert.IsType<ObjectResult>(response);
            ObjectResult result1 = Assert.IsType<ObjectResult>(response1);

            Assert.Equal(201, result.StatusCode);
            Assert.Equal(409, result1.StatusCode);
        }
        [Fact]
        public void AddUpi_Test()
        {
            CardDTO upiDTO = new CardDTO()
            {
                Name = "Paytm UPi",
                CardNo = "Upi@ybl",
                Id = Guid.NewGuid()
            };
            IActionResult response = _userController.AddPaymentCard(upiDTO);
            IActionResult response1 = _userController.AddPaymentCard(upiDTO);

            ObjectResult result = Assert.IsType<ObjectResult>(response);
            ObjectResult result1 = Assert.IsType<ObjectResult>(response1);

            Assert.Equal(201, result.StatusCode);
            Assert.Equal(409, result1.StatusCode);
        }

        [Fact]
        public void GetUserDetails_Test()
        {

            IActionResult response = _userController.GetUserDetails();

            OkObjectResult result = Assert.IsType<OkObjectResult>(response);

            Assert.Equal(200, result.StatusCode);
        }
        [Fact]
        public void DeleteAccount_Test()
        {
            ActionResult response = _userController.DeleteAccount();

            OkObjectResult result = Assert.IsType<OkObjectResult>(response);

            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void GetPaymentDetails_Test()
        {
            IActionResult response = _userController.GetPaymentDetails();
            OkObjectResult result = Assert.IsType<OkObjectResult>(response);
            Assert.Equal(200, result.StatusCode);
        }
        
        [Fact]
        public void UpdateCardDetails_Test()
        {
            UpdateCardDTO updateCardDTO = new UpdateCardDTO()
            {
                Id=Guid.Parse("26526fb6-2c2d-4e51-bd18-15fb50ab30be"),
                CardNo = "789787676768768",
                ExpiryDate = "01/23",
                Name = "jhlhjhl",
            };
            UpdateCardDTO updateCardDTO1 = new UpdateCardDTO()
            {
                Id = Guid.Parse("36526fb6-2c2d-4e51-bd18-15fb50ab30be"),
                CardNo = "789787676768768",
                ExpiryDate = "01/23",
                Name = "jhlhjhl",
            };
            IActionResult response = _userController.UpdateCardDetails(updateCardDTO);
            IActionResult response1 = _userController.UpdateCardDetails(updateCardDTO1);

            OkObjectResult result = Assert.IsType<OkObjectResult>(response);
            ObjectResult result1 = Assert.IsType<ObjectResult>(response1);

            Assert.Equal(200, result.StatusCode);
            Assert.Equal(404, result1.StatusCode);
        }

        [Fact]
        public void UpdateUpiDetails_Test()
        {
            UpdateUpiDTO updateUpiDTO = new UpdateUpiDTO()
            {
                Id = Guid.Parse("a334b297-3cc6-4d30-a304-0d95f7299064"),
               
                Upi = "jljbjk",
                Name = "jnjnjnjk",
            };
            UpdateUpiDTO updateUpiDTO1 = new UpdateUpiDTO()
            {
                Id = Guid.Parse("a334b297-3cc6-4d30-a304-0d95f7299065"),

                Upi = "jljbjk",
                Name = "jnjnjnjk",
            };
            IActionResult response = _userController.UpdateUpiDetails(updateUpiDTO);
            IActionResult response1 = _userController.UpdateUpiDetails(updateUpiDTO1);

            OkObjectResult result = Assert.IsType<OkObjectResult>(response);
            ObjectResult result1 = Assert.IsType<ObjectResult>(response1);

            Assert.Equal(200, result.StatusCode);
            Assert.Equal(404, result1.StatusCode);
        }
        
        
        public void DisposeDB()
        {
            _context.Dispose();
        }

    }
}
