using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace api_enterprise.Models.ResourceModels
{
    [SwaggerSchema("Input parameters to create an enterprise.")]
    public class EnterprisePost
    {
        [Required]
        [SwaggerSchema("Unique ID for the region.")]
        public int? RegionId { get; set; }
        [Required]
        [SwaggerSchema("Name of the enterprise.")]
        public string Summary { get; set; } = null!;
        [Required]
        [SwaggerSchema("Description of the enterprise.")]
        public string Description { get; set; } = null!;
        [Required]
        [SwaggerSchema("Country of the enterprise.")]
        public int CountryId { get; set; }
        [Required]
        [SwaggerSchema("Price of the enterprise.")]
        public int? Price { get; set; }
        [Required]
        [SwaggerSchema("Justification of the seller.")]
        public string? Justification { get; set; }
        [Required]
        [SwaggerSchema("Financing of the enterprise.")]
        public string? VendorFinancing { get; set; }
        [Required]
        [SwaggerSchema("Vendor implication of the enterprise.")]
        public string? VendorImplication { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DatePublished { get; set; }
        public int MemberId { get; set; }

    }
}
