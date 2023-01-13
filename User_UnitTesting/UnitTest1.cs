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
    public class UnitTest1 //: IHttpClientFactory
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
                    options.IncludeScopes = true; //AddAuthentication
                });
            });
            IHost host = hostBuilder.Build();
            ILogger<UserController> _logger = host.Services.GetRequiredService<ILogger<UserController>>();
          
            MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Mappers());
            });
            var _httpContextAccessor = new HttpContextAccessor();

            // need to have access to the context
            var claim = new Claim("role", "User");
            var claim1 = new Claim("Id", "uusr");
            var identity = new ClaimsIdentity(new[] { claim, claim1 }, "BasicAuthentication"); // this uses basic auth
            var principal = new ClaimsPrincipal(identity);
            _httpContextAccessor.HttpContext.User = principal;


           // _httpContextAccessor.HttpContext.User.Identities.


            //_httpContextAccessor.HttpContext.User.AddIdentity(
            //    new System.Security.Claims.ClaimsIdentity(
            //        new Claim[]
            //        {
            //            new Claim(),
            //            new Claim("Id")
            //        }
            //        )
            //    );
                


            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
            DbContextOptions<UserContext> options = new DbContextOptionsBuilder<UserContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new UserContext(options);
            _context.Database.EnsureCreated();
           
            _repository = new UserRepository(_context);
            
            _userService = new UserServices(_repository, _mapper, _httpContextAccessor);

            _userController = new UserController(_userService, _logger);
            addData();
            _context.Database.EnsureCreated();
        }
    public void addData()
        {
            string path = @"C:\Users\Hp\source\repos\User_Microservice\User_Microservice\Entity\UnitTestFiles\UserData.csv";
            string ReadCSV = File.ReadAllText(path);
            List<User> users = new List<User>();
            var data = ReadCSV.Split('\r');
            // var list = new List<RefTerm>();
            foreach (string item in data)
            {
                string[] row = item.Split(",");
                Guid Id = Guid.NewGuid();//Guid.Parse("8d0c1df7-a887-4453-8af3-799e4a7ed013");
                UserSecret userSecret = new UserSecret { Id = Guid.NewGuid(), UserId = Id, Password = row[0] };
                User user = new User { Role = row[1], FirstName = row[2], LastName = row[3], EmailAddress = row[4], Id = Id 
                ,Address =new Address(),
                Payment =new List<Payment>(),
                Phone = new Phone(),
                UserSecret=new UserSecret()
                };
                Phone phone = new Phone { Id = Guid.NewGuid(), PhoneNumber = row[5], Type = row[6], UserId = Id };
                Address address = new Address { Id = Guid.NewGuid(), Line1 = row[7], Line2 = row[8], City = row[9], Zipcode = row[10], StateName = row[11], Country = row[12], Type = row[13], UserId = Id };
                Payment payment = new Payment {Id=Guid.NewGuid(),UserId=Id,Name=row[14],CardNo=row[15],ExpiryDate=row[16] };
                Payment payment1 = new Payment {Id=Guid.NewGuid(),UserId=Id,Name=row[17],CardNo=row[18],ExpiryDate=null };
                user.Address = address;
                user.Phone = phone;
                user.UserSecret = userSecret;
                user.Payment.Add(payment);
                user.Payment.Add(payment1);
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
            Guid id = Guid.NewGuid();
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
            var result = await _userController.SignUp(user);

            var r = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, r.StatusCode);
        }

        [Fact]
        public void UpdateUser_Test()
        {
            UpdateUserDTO updateUserDTO = new UpdateUserDTO()
            {
                Id = Guid.Parse("8d0c1df7-a887-4453-8af3-799e4a7ed013"),
                Password = "User1@23",
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
                Phone = new PhoneDTO()
                {
                    PhoneNumber = "8142355769",
                    Type = "home",
                },
            };
            var result =  _userController.UpdateUser(updateUserDTO);
            var r = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, r.StatusCode);
            DisposeDB();
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
            var x = _userController.AddPaymentCard(cardDTO);
            var r = Assert.IsType<ObjectResult>(x);
            Assert.Equal(201, r.StatusCode);
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
            var x = _userController.AddPaymentCard(upiDTO);
            var r = Assert.IsType<ObjectResult>(x);
            Assert.Equal(201, r.StatusCode);


        }

        [Fact]
        public void GetUserDetails_Test()
        {

            var x = _userController.GetUserDetails(Guid.Parse("8d0c1df7-a887-4453-8af3-799e4a7ed013"));
            var r = Assert.IsType<ObjectResult>(x);
            Assert.Equal(201, r.StatusCode);
        }
        [Fact]
        public void DeleteAccount_Test()
        {
            var x = _userController.DeleteAccount(Guid.Parse("8d0c1df7-a887-4453-8af3-799e4a7ed013"));
            var r = Assert.IsType<ObjectResult>(x);
            Assert.Equal(201, r.StatusCode);
        }

        [Fact]
        public void GetPaymentDetails_Test()
        {
            var x = _userController.GetPaymentDetails(Guid.Parse("8d0c1df7-a887-4453-8af3-799e4a7ed013"));
            var r = Assert.IsType<ObjectResult>(x);
            Assert.Equal(201, r.StatusCode);
        }
        
        [Fact]
        public void UpdateCardDetails_Test()
        {
            UpdateCardDTO updateCardDTO = new UpdateCardDTO()
            {
                CardNo = "789787676768768",
                ExpiryDate = "01/23",
                Name = "jhlhjhl",
            };
            var x = _userController.UpdateCardDetails(updateCardDTO);
            var r = Assert.IsType<ObjectResult>(x);
            Assert.Equal(201, r.StatusCode);
        }

        [Fact]
        public void UpdateUpiDetails_Test()
        {
            UpdateUpiDTO updateUpiDTO = new UpdateUpiDTO()
            {
                Upi = "jljbjk",
                Name = "jnjnjnjk",
            };
            var x = _userController.UpdateUpiDetails(updateUpiDTO);
            var r = Assert.IsType<ObjectResult>(x);
            Assert.Equal(200, r.StatusCode);
        }
        
        [Fact]
        public void CheckOutDetails_Test()
        {
            CheckOutCart checkOutCart = new CheckOutCart()
            {
                
            };
            var x = _userController.CheckOutDetails(checkOutCart);
            var r = Assert.IsType<ObjectResult>(x);
            Assert.Equal(200, r.StatusCode);
        }
        [Fact]
        public void GetCheckOutDetails_Test()
        {
            CheckOutCart checkOutCart = new CheckOutCart()
            {

            };
            var x = _userController.GetCheckOutDetails(checkOutCart);
            var r = Assert.IsType<ObjectResult>(x);
            Assert.Equal(200, r.StatusCode);
        }
        public void DisposeDB()
        {
            _context.Dispose();
        }

    }
}
