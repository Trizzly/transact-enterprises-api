using System;
using System.Collections.Generic;

namespace api_enterprise.Models
{
    public partial class Member
    {
        public Member()
        {
            Enterprises = new HashSet<Enterprise>();
        }

        public int Id { get; set; }
        public string? MemberName { get; set; }
        public string AddressStreet { get; set; } = null!;
        public string AddressNumber { get; set; } = null!;
        public string AddressCity { get; set; } = null!;
        public string AddressCodePostal { get; set; } = null!;
        public string? PhoneHome { get; set; }
        public string? PhoneCell { get; set; }
        public string? PhoneBusiness { get; set; }
        public string? Fax { get; set; }

        public virtual ICollection<Enterprise> Enterprises { get; set; }
    }
}
