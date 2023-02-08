namespace api_enterprise.Models.ResourceModels
{
    public class EnterpriseResult
    {
        public int Id { get; set; }
        public string Summary { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int? Price { get; set; }
        public string? Region { get; set; }
        public string? Country { get; set; }
        public string? Member { get; set; }
    }
}
