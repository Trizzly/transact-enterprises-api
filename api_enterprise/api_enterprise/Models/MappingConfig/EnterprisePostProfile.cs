using api_enterprise.Models.ResourceModels;
using AutoMapper;

namespace api_enterprise.Models.MappingConfig
{
    public class EnterprisePostProfile : Profile
    {
        public EnterprisePostProfile()
        {
            CreateMap<EnterprisePost, Enterprise>();
        }
    }
}
