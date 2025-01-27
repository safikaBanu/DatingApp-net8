using System;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

// [ApiController]
// [Route("api/[Controller]")] //api/user(controller)
public class UserController(DataContext context) : BaseApiController
{
    //private readonly DataContext _context = context;
[AllowAnonymous]
    [HttpGet] // We can only have one of each Request Type per controller
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await context.Users.ToListAsync();
        return users;
    }
[Authorize]
    [HttpGet("{id:int}")] // /api/users/3 --> Brackets are needed for the dynamic ID of the users and :int is for type safety
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var users = await context.Users.FindAsync(id);

        if(users == null)return NotFound();

        return users;
    }
}
