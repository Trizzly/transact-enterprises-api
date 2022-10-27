using System;
using System.Collections.Generic;

namespace api_enterprise.Models
{
    public partial class Region
    {
        public Region()
        {
            Enterprises = new HashSet<Enterprise>();
        }

        public int RegionId { get; set; }
        public string RegionName { get; set; } = null!;

        public virtual ICollection<Enterprise> Enterprises { get; set; }
    }
}
