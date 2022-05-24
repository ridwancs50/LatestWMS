using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatestWMS.DTOs
{
    public class UpdateUserDTO
    {
        //public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTimeOffset DateLastModified { get; set; }
        public string Password { get; set; }
        public string FullAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
