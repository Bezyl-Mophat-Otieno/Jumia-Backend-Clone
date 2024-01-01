using AuthMS.Data.Dtos;
using AuthMS.Services.Iservice;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUser _userservice;
        private readonly IMapper _mapper;
        private readonly ResponseDTO _response;
        public AuthController(IUser userservice, IMapper mapper)
        {
            _userservice = userservice;
            _mapper = mapper;
            _response = new ResponseDTO();


        }


        [HttpPost("register")]

        public async Task<ActionResult<ResponseDTO>> RegisterUser(RegisterUserDTO newuser)
        {

            var response = await _userservice.RegisterUser(newuser);

            if (string.IsNullOrEmpty(response))
            {
                _response.Result = "User Registered Successfully";
                return Created("", _response);
            }
            _response.ErrorMessage = response;
            _response.Issuccess = false;
            return BadRequest(_response);

        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseDTO>> LoginUser(LoginRequestDTO loginRequest)
        {
            var user = await _userservice.GetUserByEmail(loginRequest.Email);

            if (user == null)
            {
                _response.ErrorMessage = "User Does Not Exist";
                return BadRequest(_response);

            }
            var res = await _userservice.LoginUser(loginRequest);

            _response.Result = res;
            return Ok(_response);

        }

        [HttpPost("assignrole")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<ResponseDTO>> AssignRole(AssignRoleDTO assignRoleDTO)
        {

            var res = await _userservice.AssignUserRole(assignRoleDTO);

            if (res)
            {
                _response.Result = res;
                return Ok(_response);
            }

            _response.ErrorMessage = "User failed to be assigned the Role";
            _response.Result = res;
            return BadRequest(_response);

        }

        [HttpGet("{Id}")]

        public async Task<ActionResult<ResponseDTO>> GetUser(Guid Id)
        {
            var user = await _userservice.GetUserById(Id);

            if(user == null)
            {
                _response.ErrorMessage = "Not Found";

                return NotFound(_response);
            }

            _response.Result= user;
            return Ok(_response);

        }
    }
}
