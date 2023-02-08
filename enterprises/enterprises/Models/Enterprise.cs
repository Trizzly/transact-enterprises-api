using System;
using System.Collections.Generic;

namespace api_enterprise.Models
{
    public partial class Enterprise
    {
        public int Id { get; set; }
        public string Summary { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int RegionId { get; set; }
        public int CountryId { get; set; }
        public int Price { get; set; }
        public string? Justification { get; set; }
        public string? VendorFinancing { get; set; }
        public string? VendorImplication { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DatePublished { get; set; }
        public byte[]? TimeStamp { get; set; }
        public int MemberId { get; set; }

        public virtual Country Country { get; set; } = null!;
        public virtual Member Member { get; set; } = null!;
        public virtual Region Region { get; set; } = null!;
    }
}
