using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
//firstly we setup the identity
//using identity users will be able to register and log into the application by supping a username and password 

//do 4 steps now wards 
//create new connection string 
//create new db context with roles(dataseed)
//inject Dbcontext and identity(asp.net core Identity)
//Run EF core migrations 

//Talking about roles :
//deal with 2 users role in our application : a reader and writer role or a user can can perform both the roles 


//In this we are going to create a new controller which will be auth controller and we will create the new register method 
//so that users can request a user to get created inside our identity database 
namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepositary tokenRepositary;

        public AuthController(UserManager<IdentityUser> userManager , ITokenRepositary tokenRepositary)
        {
            this.userManager = userManager;
            this.tokenRepositary = tokenRepositary;
        }


        //post method 
        ///api/auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto) 
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };
            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if(identityResult.Succeeded)
            {
                //add roles to this users 
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if(identityResult.Succeeded)
                    {
                        return Ok("user was register ! , please login ");
                    }

                }

            }
            return BadRequest("Something went Wrong");

        }


        //post method //api/auth/Login
        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            //first check the username 
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

            if (user != null)
            {
                //check the username with password 
                var checkPasswordResult = await userManager.CheckPasswordAsync(user , loginRequestDto.Password);
                if (checkPasswordResult)
                {
                    //get roles for this user 

                    var roles = await userManager.GetRolesAsync(user);

                    if(roles != null)
                    {
                        var jwttoken = tokenRepositary.CreateJwtToken(user, roles.ToList());
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwttoken
                        };
                        return Ok(jwttoken);

                    }
                    //create a token 

                }
            }
            return BadRequest("User or Password incorrect");
        }
    }
}
