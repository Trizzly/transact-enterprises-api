using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace api_enterprise.Models.ResourceModels
{
    [SwaggerSchema("Input parameters to modify an enterprise.")]
    public class EnterprisePut
    {
        [Required]
        [SwaggerSchema("Unique ID for the enterprise.")]
        public int Id { get; set; }
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
    }
}
