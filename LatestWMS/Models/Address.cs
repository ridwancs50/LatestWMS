using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LatestWMS.Models
{
    public class Address
    {
        public Address()
        {

        }

        public Address (string fullAddress, string city, string state, string country)
        {
            FullAddress = fullAddress;
            City = city;
            State = state;
            Country = country;
        }

        [Key]
        public Guid Id { get; set; }
        public string FullAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        //public User user { get; set; }

    }
}
