using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatestWMS.Models;

namespace LatestWMS.DTOs
{
    public class TransferDTO
    {
        public TransactionType Transactiontype { get; set; }// shd remove this n let it just reflect in d controller. Thinking out loud.
        public string BeneficiaryBank { get; set; }
        public string BeneficiaryAccount { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Amount { get; set; }
        public string Narration { get; set; }
        public string Pin { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
    }
}
