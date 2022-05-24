using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatestWMS.Models;

namespace LatestWMS.DTOs
{
    public class AirtimeDTO
    {
        public TransactionType Transactiontype { get; set; }// shd remove this n let it just reflect in d controller. Thinking out loud.
        public string Network { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
    }
}