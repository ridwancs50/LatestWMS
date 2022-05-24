using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LatestWMS.Models
{
    [Table("Transactions")]
    public class Transaction 
       
    {
        public Transaction()
        {

        }

        public Transaction (string beneficiaryBank, decimal amount, string phoneNumber, string narration, string network, string serviceProvider)
        {
            Amount = amount;
            //BeneficiaryAccount = beneficiaryAccount;
            BeneficiaryBank = beneficiaryBank;
            PhoneNumber = phoneNumber;
            Narration = narration;
            Network = network;
            ServiceProvider = serviceProvider;
        }
        [Key]
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string BeneficiaryBank { get; set; }
        public TransactionType Transactiontype { get; set; }
        public TransactionStatus Transactionstatus { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public Account BeneficiaryAccount { get; set; }
        public string PhoneNumber { get; set; }
        public string CardNumber { get; set; }
        public string Narration { get; set; }
        public Account Account { get; set; }
        public string Network { get; set; }
        public string ServiceProvider { get; set; }
    }
}

