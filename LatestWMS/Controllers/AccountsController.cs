using LatestWMS.Context;
using LatestWMS.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LatestWMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AccountsController : ControllerBase
    {

        private WMSContext _dbContext;
        public AccountsController(WMSContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("GetAccounts")]

        public IActionResult Get(int size = 10, int page = 1)
        {
            try
            {
                var account = _dbContext.Accounts.Skip((page - 1) * size).Take(size).ToList();
                if (account == null)
                {
                    return StatusCode(404, "Accounts not found");
                }
                return Ok(account);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAccount")]

        public IActionResult GetAccount(string acc)
        {
            try
            {
                var account = _dbContext.Accounts.SingleOrDefault(y => y.AccountNumber == acc);
                if (account == null)
                {
                    return StatusCode(404, "Account not found");
                }
                return Ok(account);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("UpdateAccount")]

        public ActionResult<updateAccountDTO> UpdateAccount(updateAccountDTO account)
        {
            var existingaccount = _dbContext.Accounts.FirstOrDefault(x => x.AccountNumber == account.AccountNumber);
            try
            {
                if (existingaccount == null)
                {
                    return StatusCode(404, "Account does not exist");
                }
                existingaccount.OverallLimit = account.OverallLimit;
                existingaccount.BlockedAmount = account.BlockedAmount;
                existingaccount.DateLastModified = DateTimeOffset.UtcNow;
                existingaccount.PinHash = account.PinHash;

                _dbContext.Entry(existingaccount).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {

                return StatusCode(500, "An error has occured");
            }
           // var accounts = _dbContext.Accounts.ToList();
            return Ok(existingaccount);
        }

        [HttpDelete("DeleteAccounts")]

        public IActionResult DeleteAccount([FromRoute] string Id)
        {
            var account = _dbContext.Accounts.FirstOrDefault(x => x.AccountNumber == Id);
            try
            {
                if (account == null)
                {
                    return StatusCode(404, "Account does not exist");
                }
                _dbContext.Entry(account).State = EntityState.Deleted;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
           // var accounts = _dbContext.Accounts.ToList();
            return Ok(account);
        }

    }
}
