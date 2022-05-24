using LatestWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatestWMS.DTOs
{
    public class FundWallet
    {
        public TransactionType Transactiontype { get; set; }// shd remove this n let it just reflect in d controller. Thinking out loud.
        public string DepositorBank { get; set; }
        public string DepositorAccount { get; set; }
        public string PhoneNumber { get; set; }
        //public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Narration { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
    }
}
