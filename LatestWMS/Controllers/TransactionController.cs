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
using Microsoft.Extensions.Logging;
/*using Microsoft.Extensions.Options;
*/using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LatestWMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "AppUser")]
    public class TransactionController : ControllerBase
    {
        private readonly WMSContext _dbcontext;
        private readonly UserManager<LatestWMS.Models.ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(
            WMSContext dbcontext,
            UserManager<LatestWMS.Models.ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<TransactionController> logger
        )
        {
            _logger = logger;
            _dbcontext = dbcontext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("SetPin")]
        public async Task<ActionResult<CreatePinDTOs>> CreatePin(CreatePinDTOs pin)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _dbcontext.AppUser.Include(x => x.Account).SingleOrDefaultAsync(x => x.Email == userId);
            if (user == null)
            {
                return BadRequest(new RegistrationResponse()
                {
                    Errors = new List<string>() {
                                "Please Login!"
                            },
                    Success = false
                });
            }

            if (user != null)
            {
                var acc = user.Account;
                acc.PinHash = BCrypt.Net.BCrypt.HashPassword(pin.Pin);
                acc.PinCreationDate = pin.CreationDate;
                
                //_dbcontext.Accounts.Add(acc);
                _dbcontext.SaveChanges();

                return StatusCode(200, "Pin set Successful");
            }

            else
            {
                return StatusCode(404, "did not run");
            }

        }

        [HttpPost("ChangePin")]
        public async Task<ActionResult<CreatePinDTOs>> ChangePin(UpdatePinDTOs newpin)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _dbcontext.AppUser.Include(x => x.Account).SingleOrDefaultAsync(x => x.Email == userId);
            if (user == null)
            {
                return BadRequest(new RegistrationResponse()
                {
                    Errors = new List<string>() {
                                "Please Login!"
                            },
                    Success = false
                });
            }

            if (user != null)
            {
                var acc = user.Account;
                var currentPinHash = acc.PinHash;
                acc.PinHash = BCrypt.Net.BCrypt.ValidateAndReplacePassword(newpin.CurrentPin, currentPinHash, newpin.NewPin);

                _dbcontext.SaveChanges();

                return StatusCode(200, "Pin set Successful");
            }

            else
            {
                return StatusCode(404, "did not run");
            }

        }

        [HttpPost("MakeTransfer")]

        public async Task<ActionResult<TransferDTO>> Transfer(TransferDTO trans)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _dbcontext.AppUser.Include(x => x.Account).SingleOrDefaultAsync(x => x.Email == userId);
            if (user == null)
            {
                return BadRequest(new RegistrationResponse()
                {
                    Errors = new List<string>() {
                                "Please Login!"
                            },
                    Success = false
                });

            }
            if (user != null)
            {
                var beneficiaryAccount = _dbcontext.Accounts.SingleOrDefault(y => y.AccountNumber == trans.BeneficiaryAccount);

                if (beneficiaryAccount == null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                                "Invalid Account Number"
                            },
                        Success = false
                    });
                }
                var tmpAccount = user.Account;
                var accountPin = tmpAccount.PinHash;
                Transaction newtransfer = new Transaction();
                Account account = new Account();

                //account.AccountNumber = trans.AccountNumber;
                newtransfer.Amount = trans.Amount;
                newtransfer.BeneficiaryAccount = beneficiaryAccount;
                newtransfer.Account = tmpAccount;
                newtransfer.BeneficiaryBank = trans.BeneficiaryBank;
                newtransfer.PhoneNumber = trans.PhoneNumber;
                newtransfer.Narration = trans.Narration;
                newtransfer.TransactionDate = DateTime.Now;

                if (!BCrypt.Net.BCrypt.Verify(trans.Pin, accountPin))
                {
                    return null;
                }

                var verifyBalance = tmpAccount.CurrentBalance - trans.Amount;

                if (trans.Amount > 0 && trans.Amount > verifyBalance)
                {
                    return StatusCode(404, "Insufficient Balance");
                }
                tmpAccount.CurrentBalance -= trans.Amount;
                beneficiaryAccount.CurrentBalance += trans.Amount;

                // _dbcontext.Entry(newtransfer).State = EntityState.Modified;
                _dbcontext.Transactions.Add(newtransfer);
                _dbcontext.SaveChanges();

                return StatusCode(200, "Transfer Successful");
            }
            else
            {
                return StatusCode(404, "did not run");
            }

        }

        [HttpGet("GetTransactions")]
        public async Task<IActionResult> Get(int size = 10, int page = 1)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var user = await _dbcontext.AppUser.Include(x => x.Account).SingleOrDefaultAsync(x => x.Email == userId);
                if (user != null)
                {
                    var userAccount = user.Account;
                    var trans = _dbcontext.Transactions.Include(x => x.Account).Where(x => x.Account.AccountNumber == userAccount.AccountNumber).Skip((page - 1) * size).Take(size).ToList();
                    return Ok(trans);
                }
                else
                {
                    return StatusCode(404, "No Transaction found");
                }

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetBalance")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var user = await _dbcontext.AppUser.Include(x => x.Account).SingleOrDefaultAsync(x => x.Email == userId);
                var userAccountBalance = user.Account.CurrentBalance;
                return Ok(userAccountBalance);

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("BuyAirtime")]

        public async Task<ActionResult<AirtimeDTO>> BuyAirtime(AirtimeDTO air)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _dbcontext.AppUser.Include(x => x.Account).SingleOrDefaultAsync(x => x.Email == userId);
            if (user == null)
            {
                return BadRequest(new RegistrationResponse()
                {
                    Errors = new List<string>() {
                                "Please Login!"
                            },
                    Success = false
                });

            }
            if (user != null)
            {
                var phoneNumber = _dbcontext.AppUser.SingleOrDefault(y => y.PhoneNumber == air.PhoneNumber);
                if (phoneNumber == null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                                "Phone number is wrong"
                            },
                        Success = false
                    });
                }
                var tmpAccount = user.Account;
                Transaction newtransfer = new Transaction();
                Account account = new Account();

                newtransfer.Amount = air.Amount;
                newtransfer.PhoneNumber = air.PhoneNumber;
                newtransfer.Account = tmpAccount;
                newtransfer.Network = air.Network;
                newtransfer.TransactionDate = DateTime.Now;

                var verifyBalance = tmpAccount.CurrentBalance - air.Amount;

                if (air.Amount > 0 && air.Amount > verifyBalance)
                {
                    return StatusCode(404, "Insufficient Balance");
                }
                tmpAccount.CurrentBalance -= air.Amount;
                phoneNumber.PhoneNumber += air.Amount;

                _dbcontext.Transactions.Add(newtransfer);
                _dbcontext.SaveChanges();

                return StatusCode(200, "Airtime Purchase Successful");
            }
            else
            {
                return StatusCode(404, "did not run");
            }
        }

        [HttpPost("FundWallet")]

        public async Task<ActionResult<FundWallet>> Transfer(FundWallet fund)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _dbcontext.AppUser.Include(x => x.Account).SingleOrDefaultAsync(x => x.Email == userId);
            if (user == null)
            {
                return BadRequest(new RegistrationResponse()
                {
                    Errors = new List<string>() {
                                "Please Login!"
                            },
                    Success = false
                });

            }
            if (user != null)
            {
                var depositorAccount = _dbcontext.Accounts.SingleOrDefault(y => y.AccountNumber == fund.DepositorAccount);
                if (depositorAccount == null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                                "Invalid Account Number"
                            },
                        Success = false
                    });
                }
                var tmpAccount = user.Account;
                Transaction newtransfer = new Transaction();
                Account account = new Account();

                newtransfer.Amount = fund.Amount;
                newtransfer.BeneficiaryAccount = depositorAccount;
                newtransfer.Account = tmpAccount;
                newtransfer.BeneficiaryBank = fund.DepositorBank;
                newtransfer.PhoneNumber = fund.PhoneNumber;
                newtransfer.Narration = fund.Narration;
                newtransfer.TransactionDate = DateTime.Now;

                var verifyBalance = tmpAccount.CurrentBalance - fund.Amount;

                if (fund.Amount > 0 && fund.Amount > verifyBalance)
                {
                    return StatusCode(404, "Insufficient Balance");
                }
                tmpAccount.CurrentBalance += fund.Amount;
                depositorAccount.CurrentBalance -= fund.Amount;

                // _dbcontext.Entry(newtransfer).State = EntityState.Modified;
                _dbcontext.Transactions.Add(newtransfer);
                _dbcontext.SaveChanges();

                return StatusCode(200, "Wallet successfully funded");
            }
            else
            {
                return StatusCode(404, "did not run");
            }
        }
    }
}
