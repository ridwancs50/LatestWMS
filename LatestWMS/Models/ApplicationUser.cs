﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatestWMS.Models;

namespace LatestWMS.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DateLastModified { get; set; }
        public Address Address { get; set; }
        public Account Account { get; set; }

    }
}
