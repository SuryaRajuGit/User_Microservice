using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User_Microservice.Contracts;
using User_Microservice.Entity.Dto;

namespace User_Microservice.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices _userService;

        public UserController(IUserServices userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("api/login/ocelot")]
        public IActionResult VerifyUser([FromBody]LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                ErrorDTO badRequest = _userService.ModelStateInvalid(ModelState);
                return BadRequest(badRequest);
            }
            ErrorDTO isEmailExists = _userService.IsEmailExists(loginDTO.EmailAddress);
            if(isEmailExists == null)
            {
                return StatusCode(404,new ErrorDTO {type="Login",description="User not found" });
            }
            LoginResponseDTO loginResponseDTO = _userService.VerifyLoginDetails(loginDTO);
            if(loginResponseDTO == null)
            {
                return StatusCode(401, new ErrorDTO { type = "Login", description = "Invalid Password" });
            }
            return Ok(loginResponseDTO);
        }

        [HttpPost]
        [Route("api/account/ocelot")]
        public IActionResult SinUp([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                ErrorDTO badRequest = _userService.ModelStateInvalid(ModelState);
                return BadRequest(badRequest);
            }

            ErrorDTO email = _userService.IsEmailExists(userDTO.EmailAddress);
            if(email != null)
            {
                return StatusCode(409, email);
            }

            ErrorDTO phone = _userService.IsPhoneExists(userDTO.Phone.PhoneNumber);
            if (phone != null)
            {
                return StatusCode(409, phone);
            }
            ErrorDTO address = _userService.IsAddressExists(userDTO.Address);
            if (phone != null)
            {
                return StatusCode(409, phone);
            }
            Guid response = _userService.SaveUser(userDTO);
            return StatusCode(201,response);
        }

        [HttpPost]
        [Route("api/add-card/ocelot")]
        public IActionResult AddPaymentCard([FromBody] CardDTO cardDTO)
        {
            if (!ModelState.IsValid)
            {
                ErrorDTO badRequest = _userService.ModelStateInvalid(ModelState);
                return BadRequest(badRequest);
            }
            ErrorDTO isCardExists = _userService.IsCardExists(cardDTO.CardNo);
            if(isCardExists != null)
            {
                return StatusCode(409, isCardExists);
            }
            Guid saveCard = _userService.SaveCard(cardDTO);

        }
      
    }
}
