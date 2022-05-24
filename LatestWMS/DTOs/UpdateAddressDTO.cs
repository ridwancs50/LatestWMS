using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatestWMS.DTOs
{
    public class UpdateAddressDTO
    {
        public Guid Id { get; set; }
        public DateTimeOffset DateLastModified { get; set; }
        public string FullAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
