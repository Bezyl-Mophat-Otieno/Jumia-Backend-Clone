using AuthMS.Data;
using AuthMS.Data.Dtos;
using AuthMS.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuthMS.Services.Iservice;
using JumiaAzureServiceBus;
using AuthMS.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace AuthMS.Services
{
    public class UserService:IUser
    {

        private readonly IMapper _mapper;
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly IJWT _jwtservice;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<RegistrationHub> _hubcontext;


        public UserService(ApplicationDBContext context 
            , IMapper mapper , RoleManager<IdentityRole> rolemanager , UserManager<ApplicationUser> usermanager , IJWT jwtservice, IConfiguration configuration , IHubContext<RegistrationHub> hubcontext)
        {
            _context = context;
                _mapper = mapper;
            _rolemanager = rolemanager;
            _usermanager = usermanager;
            _jwtservice = jwtservice;
            _configuration = configuration;
            _hubcontext = hubcontext;
            
        }
        public async Task<bool> AssignUserRole(AssignRoleDTO assign)
        {
            try {

                var user = await _context.Users.Where(x => x.Email.ToLower() == assign.Email.ToLower()).FirstOrDefaultAsync();

                if (user == null)
                {
                    return false;


                }

                // Check if role exists

                var isRoleExist = await  _rolemanager.RoleExistsAsync(assign.Role);

                if(!isRoleExist)
                {
                    // Create the Role

                    await _rolemanager.CreateAsync(new IdentityRole(assign.Role));

                    //Assign the Role

                    await _usermanager.AddToRoleAsync(user,assign.Role);

                    return true;
                    
                }

                // Assign the Role
                await _usermanager.AddToRoleAsync(user, assign.Role);
                return true;

            }
            catch (Exception ex) {


                return false;
            }
        }

        public async Task<UserDTO> GetUserByEmail(string Email)
        {
            try
            {

                var user = await _context.Users.Where(user=>user.Email == Email).FirstOrDefaultAsync();

                var mappeduser = _mapper.Map<UserDTO>(user); 


                return mappeduser;


            }
            catch (Exception)
            {
                return null;

            }
        }

        public async Task<UserDTO> GetUserById(Guid userId)
        {
            try {

                var user = await _context.Users.Where(user=>user.Id == userId.ToString()).FirstOrDefaultAsync();

                var mappeduser = _mapper.Map<UserDTO>(user);

                return mappeduser;
            
            
            }catch(Exception) {
                return null;
            
            }
        }

        public async Task<string> LoginUser(LoginRequestDTO user)
        {
            try {

                // find the user with that Email

                var userexist = await _context.Users.Where(user=>user.Email == user.Email).FirstOrDefaultAsync();

                if (userexist != null)
                {
                    // Verify Password 

                    var isValid = await _usermanager.CheckPasswordAsync(userexist, user.Password);

                    if(isValid)

                    {
                        var roles = await _usermanager.GetRolesAsync(userexist);
                        var token = _jwtservice.GetToken(userexist,roles);
                        return token;

                    }
                    else
                    {
                        return "Invalid Credentails";
                    }


                }
                else
                {
                    return $" User with this email :{user.Email} does not exist";
                }
            
            }catch(Exception ex) {


                return ex.Message;
;            
            }
        }

      

        public async Task<string> RegisterUser(RegisterUserDTO newuser)
        {
            try {

                var User = _mapper.Map<ApplicationUser>(newuser);

                var result = await _usermanager.CreateAsync(User , newuser.Password);

                if(result.Succeeded)
                {

                    // check if role exists 

                    var isRoleExist = await _rolemanager.RoleExistsAsync(newuser.Role);

                    if (!isRoleExist)
                    {

                        // Create the Role 
                        await _rolemanager.CreateAsync(new IdentityRole(newuser.Role));

                        // Assign The Role 

                        await _usermanager.AddToRoleAsync(User , newuser.Role);


                    }

                    await _usermanager.AddToRoleAsync(User, newuser.Role);

                    // Sending the message to the client 

                    await _hubcontext.Clients.Group("user").SendAsync("OnRegistration", newuser, "User Registered Successfully");

                    var publishedmessage = new UserMessageDTO()
                    {
                        Name = newuser.Name,
                        Email = newuser.Email,
                    };
                    var queue = _configuration.GetSection("ServiceBus:registrationqueue").Value;

                    var messagebus = new MessageBus();

                    await  messagebus.PublishMessage(publishedmessage , queue);
  
                    return "";

                }
                else
                {
                    return result.Errors?.FirstOrDefault().Description;
                }

                
                
            
            } catch(Exception ex) {


                return ex.Message;
            }
        }
    }
}
