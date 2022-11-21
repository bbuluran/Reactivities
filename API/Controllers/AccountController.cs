using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, 
                                SignInManager<AppUser> signInManager,
                                TokenService tokenService)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (result.Succeeded) 
            {
                return CreateUserModel(user);
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model) 
        {
            if (await _userManager.Users.AnyAsync(a => a.Email == model.Email)) 
            {
                ModelState.AddModelError("email", "Email taken");
                return ValidationProblem();
            }
                
            if (await _userManager.Users.AnyAsync(a => a.UserName == model.Username)) 
            {
                ModelState.AddModelError("username", "Username taken");
                return ValidationProblem();
            }

            // create new user
            var user = new AppUser 
            {
                DisplayName = model.DisplayName,
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded) 
            {
                return CreateUserModel(user);
            }
            else 
            {
                return BadRequest("User not registered");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            return CreateUserModel(user);
        }

        private UserDto CreateUserModel(AppUser model)
        {
            return new UserDto 
            {
                DisplayName = model.DisplayName,
                ImageUrl = null,
                Token = _tokenService.CreateToken(model),
                Username = model.UserName
            };
        }
    }
}