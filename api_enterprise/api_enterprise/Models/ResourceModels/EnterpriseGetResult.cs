using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace enterprises.Models.ResourceModels
{
    [SwaggerSchema("Object returned by the enterpriseget.")]
    public class EnterpriseGetResult
    {
        [Required]
        [SwaggerSchema("Unique Id of the enterprise.")]
        public int Id { get; set; }
        [Required]
        [SwaggerSchema("Summary of the enterprise.")]
        public string Summary { get; set; } = null!;

        [Required]
        [SwaggerSchema("Region id of the enterprise.")]
        public int RegionID { get; set; }

        [Required]
        [SwaggerSchema("^Price of the enterprise.")]
        public int Price { get; set; }
    }
}
