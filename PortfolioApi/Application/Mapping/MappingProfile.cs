using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Domain.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUserDto, User>();
        CreateMap<CreateIconDto, Icons>();
        CreateMap<CreateTechnologyDto, Technology>();
        CreateMap<CreateRoleDto, Role>();
        CreateMap<UpdateTechnologyDto, Technology>();
        CreateMap<CreateProjectDto, Project>();
        CreateMap<UpdateProjectDto, Project>();
        
        CreateMap<User, UserWithProjectNamesDto>()
            .ForMember(dest => dest.ProjectNames, opt =>
                opt.MapFrom(src => src.Projects != null
                    ? src.Projects.Where(p => !string.IsNullOrEmpty(p.Name)).Select(p => p.Name!).ToList()
                    : new List<string>()));
        
        CreateMap<Role, RoleWithTechnologiesDto>()
            .ForMember(dest => dest.IconPath, opt =>
                opt.MapFrom(src => src.Icon != null ? src.Icon.Path : null))
            .ForMember(dest => dest.Technologies, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.Technologies = src.Technologies
                    .Select(t => new TechnologySummaryDto
                    {
                        Name = t.Name ?? string.Empty,
                        IconPath = t.Icon != null ? t.Icon.Path : null
                    }).ToList();
            });
        
        CreateMap<Technology, GetTechnologyDto>()
            .ForMember(dest => dest.IconPath, opt => opt.MapFrom(src => src.Icon != null ? src.Icon.Path : null));

    }
}