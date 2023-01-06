using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using User_Microservice.Contracts;
using User_Microservice.Entity.Dto;

namespace User_Microservice.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userService;
        private readonly ILogger _logger;

        public UserController(IUserServices userService, ILogger logger)
        {
            _userService = userService;
            _logger = logger;
        }

        ///<summary>
        /// veryfies user login details
        ///</summary>
        ///<param name="loginDTO">Login details of user data</param>
        ///<returns>returns access token and its type</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("api/login/ocelot")]
        public IActionResult VerifyUser([FromBody] LoginDTO loginDTO)
        {
            _logger.LogInformation("Verifying user started");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Entered wrong data");
                ErrorDTO badRequest = _userService.ModelStateInvalid(ModelState);
                return BadRequest(badRequest);
            }
            ErrorDTO isEmailExists = _userService.IsEmailExists(loginDTO.EmailAddress);
            if (isEmailExists == null)
            {
                _logger.LogError("User not found with the details.");
                return StatusCode(404, new ErrorDTO { type = "Login", description = "User not found" });
            }
            LoginResponseDTO loginResponseDTO = _userService.VerifyLoginDetails(loginDTO);
            if (loginResponseDTO == null)
            {
                _logger.LogError("Enter invalid password.");
                return StatusCode(401, new ErrorDTO { type = "Login", description = "Invalid Password" });
            }
            _logger.LogInformation("User logged successfully.");
            return Ok(loginResponseDTO);
        }

        ///<summary>
        /// Creates new User account.
        ///</summary>
        ///<param name="userDTO">User sign up details</param>
        ///<returns>returns new User id</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("api/account")]
        public async Task<IActionResult> SignUp([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation("Sign-up user started.");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Entered wrong data");
                ErrorDTO badRequest = _userService.ModelStateInvalid(ModelState);
                return BadRequest(badRequest);
            }

            ErrorDTO email = _userService.IsEmailExists(userDTO.EmailAddress);
            if (email != null)
            {
                _logger.LogError("Email address already exist");
                return StatusCode(409, email);
            }

            ErrorDTO phone = _userService.IsPhoneExists(userDTO.Phone.PhoneNumber);
            if (phone != null)
            {
                _logger.LogError("Phone number already exist.");
                return StatusCode(409, phone);
            }
            ErrorDTO address = _userService.IsAddressExists(userDTO.Address);
            Guid response = Guid.Empty;
            if (address != null)
            {
                _logger.LogError("Address already exist.");
                return StatusCode(409, address);
            }
            try
            {
                response = await _userService.SaveUser(userDTO);
                _logger.LogInformation("New user created successfully.");
                return StatusCode(201, response);
            }
            catch
            {
                _logger.LogError("Order service unavailable");
                return StatusCode(500, "Order service unavailable");
            }

        }

        ///<summary>
        /// Updates User details.
        ///</summary>
        ///<param name="updateUserDTO">User details</param>
        ///<returns>User details Updated Successfully</returns>
        [HttpPut]
        [Route("api/account")]
        public IActionResult UpdateUser([FromBody] UpdateUserDTO updateUserDTO)
        {
            _logger.LogInformation("Update user details started");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Entered wrong data");
                ErrorDTO badRequest = _userService.ModelStateInvalid(ModelState);
                return BadRequest(badRequest);
            }
            ErrorDTO isUserExists = _userService.IsUserExists(updateUserDTO.Id);
            if (isUserExists != null)
            {
                _logger.LogError($"User id {isUserExists} not found.");
                return StatusCode(404, isUserExists);
            }
            ErrorDTO isUserDetailsAlreadyExist = _userService.IsUserDetailsAlreadyExist(updateUserDTO);
            if (isUserDetailsAlreadyExist != null)
            {
                _logger.LogError($"User details {isUserDetailsAlreadyExist} already Exist");
                return StatusCode(409, isUserDetailsAlreadyExist);
            }
            _userService.SaveUpadateUser(updateUserDTO);
            _logger.LogInformation("User details Updated Successfully");
            return Ok("User details Updated Successfully");
        }

        ///<summary>
        /// Adds payement cart to user account
        ///</summary>
        ///<param name="cardDTO">User details</param>
        ///<returns>Card id</returns>
        [HttpPost]
        [Route("api/add-card")]
        public IActionResult AddPaymentCard([FromBody] CardDTO cardDTO)
        {
            _logger.LogInformation("Add payment card deatils started.");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Entered wrong data");
                ErrorDTO badRequest = _userService.ModelStateInvalid(ModelState);
                return BadRequest(badRequest);
            }
            Guid id = Guid.Parse(this.User.Claims.First(item => item.Type == "Id").Value);
            ErrorDTO isCardExists = _userService.IsCardExists(cardDTO.CardNo,id);
            if (isCardExists != null)
            {
                _logger.LogError("Card no already exist");
                return StatusCode(409, isCardExists);
            }
            Guid saveCard = _userService.SaveCard(cardDTO, id);
            _logger.LogInformation("New paymane card added successfully");
            return StatusCode(201,saveCard);
        }
        [HttpPost]
        [Route("api/upi")]
        public IActionResult AddUpi([FromBody]UpiDTO upiDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Entered wrong data");
                ErrorDTO badRequest = _userService.ModelStateInvalid(ModelState);
                return BadRequest(badRequest);
            }
            
            ErrorDTO isUserExist = _userService.CheckUser(upiDTO.Id);
            if (isUserExist != null)
            {
                return StatusCode(404, isUserExist);
            }
            ErrorDTO isUpiExist = _userService.CheckUpi(upiDTO);
            if (isUpiExist != null)
            {
                return StatusCode(409, isUpiExist);
            }
            Guid id = _userService.SaveUpi(upiDTO);
            return StatusCode(201,id);
        }



        ///<summary>
        /// Gets user details
        ///</summary>
        ///<param name="id">User id</param>
        ///<returns>User deatils</returns>
        [HttpGet]
        [Route("api/account/{id}")]
        public IActionResult GetUserDetails([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                ErrorDTO badRequest = _userService.ModelStateInvalid(ModelState);
                return BadRequest(badRequest);
            }
            UserDetailsResponse userDetails = _userService.GetuserDetails(id);
            if (userDetails == null)
            {
                return StatusCode(404, new ErrorDTO() { type = "User", description = "User with id not found" });
            }
            return Ok(userDetails);
        }

        ///<summary>
        /// Delets User account
        ///</summary>
        ///<param name="id">User id</param>
        ///<returns>Account deleted successfully</returns>
        [HttpDelete]
        [Route("api/account/{id}")]
        public  ActionResult DeleteAccount([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                ErrorDTO badRequest = _userService.ModelStateInvalid(ModelState);
                return BadRequest(badRequest);
            }
            try
            {
                ErrorDTO isAccountDeleted =  _userService.DeleteAccount(id);
                if (isAccountDeleted != null)
                {
                    return StatusCode(404, isAccountDeleted);
                }
            }
            catch(Exception ex)
            {
                return StatusCode(404,ex.Message);
            }
            
            return Ok("Account deleted successfully");
        }
        [HttpGet]
        [Route("api/payment/account/{id}")]
        public ActionResult<PaymentDetailsResponseDTO> GetPaymentDetails([FromRoute] Guid id)
        {
            ErrorDTO isUserExist = _userService.IsUserExist(id);
            if(isUserExist != null)
            {
                return StatusCode(404,isUserExist);
            }
            PaymentDetailsResponseDTO x = _userService.GetPaymentDetails(id);
            if (x == null)
            {
                return StatusCode(204, "No Payment details added");
            }
            return Ok(x);
        }
        [HttpPut]
        [Route("api/card/account")]
        public IActionResult UpdateCardDetails([FromBody] UpdateCardDTO cardDTO)
        {
            if (!ModelState.IsValid)
            {
                ErrorDTO badRequest = _userService.ModelStateInvalid(ModelState);
                return BadRequest(badRequest);
            }
            ErrorDTO isUserDetailsExist = _userService.IsUserDetailsExist(cardDTO);
            if (isUserDetailsExist != null)
            {
                return StatusCode(404, isUserDetailsExist);
            }
            ErrorDTO x = _userService.IsCardDetailsExist(cardDTO);
            if (x != null)
            {
                return StatusCode(409, x);
            }
            return Ok("Card details updated sucessfully");
        }
        [HttpPut]
        [Route("api/upi")]
        public IActionResult UpdateUpiDetails([FromBody]UpdateUpiDTO updateUpiDTO)
        {
            if (!ModelState.IsValid)
            {
                ErrorDTO badRequest = _userService.ModelStateInvalid(ModelState);
                return BadRequest(badRequest);
            }
            ErrorDTO isUpiDetailsExist = _userService.IsUpiDetailsExist(updateUpiDTO);
            if (isUpiDetailsExist != null)
            {
                return StatusCode(404, isUpiDetailsExist);
            }
            ErrorDTO updateUpiDetails = _userService.UpdateUpiDetails(updateUpiDTO);
            if (updateUpiDetails != null)
            {
                return StatusCode(409, updateUpiDetails);
            }
            return Ok("Upi details updated successfully");
        }
        [HttpPost]
        [HttpPost]
        [Route("api/check-out/details")]
        public string CheckOutDetails([FromBody] CheckOutCart checkOutDetailsDTO )
        {
            ErrorDTO response = _userService.IsPaymentDetailsExist(checkOutDetailsDTO);
            if(response != null)
            {
                return JsonConvert.SerializeObject(response);
            }
            return null;
        }

        [HttpPost]
        [Route("api/check-out")]
        public string GetCheckOutDetails([FromBody] CheckOutCart checkOutDetailsDTO)
        {
            return _userService.GetCheckOutPaymentDetails(checkOutDetailsDTO);
        }
        
    }
}
