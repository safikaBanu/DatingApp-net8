using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTO;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace API.Controllers;

/*
 * ControllerBase is a class that has all the functionality of a controller
 * We can use ControllerBase instead of Controller
 * ControllerBase is a more lightweight version of Controller
 * ControllerBase is a class that provides a base class for an MVC controller without view support.
 */

    public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
    {
        [HttpPost("register")] //account/register

    //    public async Task<ActionResult<AppUser>> Register( string username, string password)

        public async Task<ActionResult<UserDto>> Register(Registerdto registerdto)
        {
            if(await UserExists(registerdto.UserName)) return BadRequest("Username is taken");
          
          using var hmac =  new HMACSHA512();
          var user = new AppUser
          {
            UserName = registerdto.UserName.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerdto.Password)),
            PasswordSalt = hmac.Key
          };
          context.Users.Add(user);
          await context.SaveChangesAsync();

          return new UserDto
          {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)

          };
        }


        

        private async Task<bool> UserExists(string username )
        {
            return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }
        [HttpPost("login")]
   
        public async Task<ActionResult<UserDto>> Login (LoginDto logindto)
        {

             var user = await context.Users.FirstOrDefaultAsync(x => 
                    x.UserName.ToLower() == logindto.UserName.ToLower());

          if (user == null) return Unauthorized("Invalid Username"); 

          using var hmac  = new HMACSHA512(user.PasswordSalt);

          var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(logindto.Password));

          for(int i=0; i < computedHash.Length; i++){
            if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
          }
          return new UserDto{
            Username = user.UserName,
Token = tokenService.CreateToken(user)
          };
        }
        

    }

