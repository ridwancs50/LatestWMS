using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LatestWMS.Models
{
    [Table("Accounts")]
    public class Account
    {
        Random rand = new Random();
        public Account()
        {
            AccountNumber = Convert.ToString(rand.Next(1_000_000_000, 2_000_000_000));
        }

        public Account (string accountNumber, string accountName, decimal currentBalance, decimal overallLimit, string currency, decimal blockedAmount, string pinHash)
        {
            AccountNumber = accountNumber;
            AccountName = accountName;
            CurrentBalance = currentBalance;
            Currency = currency;
            OverallLimit = overallLimit;
            BlockedAmount = blockedAmount;
            PinHash = pinHash;
            //Transactions = new List<Transaction>();
        }

        [Key]
        public string AccountNumber { get; set; }
        // public Guid Id { get; set; }
        public string AccountName { get; set; } //new
        public string PinHash { get; set; } //new
        public decimal CurrentBalance { get; set; }
        public decimal OverallLimit { get; set; }
        public string Currency { get; set; }
        public decimal BlockedAmount { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset DateLastModified { get; set; }
       // public ApplicationUser applicationUser { get; set; }
        //public IEnumerable<Transaction> Transactions { get; set; }

    }
}
