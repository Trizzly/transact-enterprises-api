using System;
using System.Collections.Generic;

namespace api_enterprise.Models
{
    public partial class Country
    {
        public Country()
        {
            Enterprises = new HashSet<Enterprise>();
        }

        public int Id { get; set; }
        public string? CountryName { get; set; }

        public virtual ICollection<Enterprise> Enterprises { get; set; }
    }
}
