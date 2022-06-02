using LatestWMS.Context;
using LatestWMS.DTOs;
using LatestWMS.DTOs.Responses;
using LatestWMS.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LatestWMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private WMSContext _dbContext;
        public UserController(WMSContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _dbContext.AppUser.FirstOrDefaultAsync(x => x.UserName == id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers(int size = 10, int page = 1)
        {
            var users = await _dbContext.AppUser.Skip((page - 1) * size).Take(size).ToListAsync();
            return Ok(users);
        }

        [HttpPut("UpdateUser")]

        public ActionResult<UpdateUserDTO> Update(UpdateUserDTO user)
        {
            try
            {
                Address updaddr = new Address();
                var upduser = _dbContext.AppUser.FirstOrDefault(x => x.Id == user.UserName);
                if (upduser == null)
                {
                    return StatusCode(404, "User not found");
                }
                upduser.FirstName = user.FirstName;
                upduser.LastName = user.LastName;
                upduser.PhoneNumber = user.PhoneNumber;
                upduser.Email = user.Email;
                upduser.Address = updaddr;
                upduser.DateLastModified = DateTime.UtcNow;
                updaddr.FullAddress = user.FullAddress;
                updaddr.City = user.City;
                updaddr.State = user.State;
                updaddr.Country = user.Country;

                _dbContext.Entry(upduser).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
            return NoContent();
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string Username)
        {
            var userdel = await _dbContext.AppUser.FirstOrDefaultAsync(x => x.UserName == Username);
            var r_token = _dbContext.RefreshTokens.Where(x => x.UserId == userdel.Id);

            if (r_token != null)
            {
               _dbContext.RefreshTokens.RemoveRange(r_token);
            }

            if (userdel == null)
            {
                return BadRequest(new RegistrationResponse()
                {
                    Errors = new List<string>() {
                                "user not found"
                            },
                    Success = false
                });
            }
            _dbContext.AppUser.Remove(userdel);
            _dbContext.SaveChanges();
            return Ok(userdel.Id);
        }
    }
}
