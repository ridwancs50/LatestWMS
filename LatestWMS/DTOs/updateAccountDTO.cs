using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatestWMS.DTOs
{
    public class updateAccountDTO
    {
        //public Guid Id { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; } //new
        public string PinHash { get; set; } //new
        public decimal OverallLimit { get; set; }
        public decimal BlockedAmount { get; set; }
        public DateTimeOffset DateLastModified { get; set; }
    }
}
