using AutoMapper;

namespace SalaryAdvanced.Application.Mappings
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Domain.Entities.Department, DTOs.DepartmentDTO>().ReverseMap();
            CreateMap<Domain.Entities.Department, DTOs.GetDepartmentDTO>().ReverseMap();
            CreateMap<Domain.Entities.SalaryAdvanceRequest, DTOs.CreateRequestDTO>().ReverseMap();
            CreateMap<Domain.Entities.SalaryAdvanceRequest, DTOs.GetRequestDTO>().ReverseMap();
        }
    }
}
