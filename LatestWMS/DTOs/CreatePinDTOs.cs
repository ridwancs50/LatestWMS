using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatestWMS.DTOs
{
    public class CreatePinDTOs
    {
        public string Pin { get; set; }
        public DateTimeOffset CreationDate { get; set; } = DateTime.UtcNow;
        public DateTimeOffset DateLastModified { get; set; } = DateTime.UtcNow;
    }
}
