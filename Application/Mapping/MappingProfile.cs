using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Domain.Entities;
using PortfolioApi.Domain.ValueObjects;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.Config, opt => opt.Ignore());
        CreateMap<CreateIconDto, Icons>();
        CreateMap<CreateTechnologyDto, Technology>();
        CreateMap<CreateRoleDto, Role>();
        CreateMap<CreateProjectDto, Project>()
            .ForMember(dest => dest.Icon, opt => opt.Ignore());;
        CreateMap<UpdateProjectDto, Project>()
            .ForMember(dest => dest.Technologies, opt => opt.Ignore())
            .ForMember(dest => dest.Icon, opt => opt.Ignore())
            .ForMember(dest => dest.Config, opt => opt.Ignore());


        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.Config, opt => opt.Ignore());

        CreateMap<UpdateTechnologyDto, Technology>();
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

        CreateMap<CreateUserRoleProgressDto, UserRoleProgress>()
            .ForMember(dest => dest.Projects, opt => opt.Ignore());
        CreateMap<UserRoleProgress, CreateUserRoleProgressDto>();

        CreateMap<UserRoleProgress, GetUserRoleProgressDto>();

        CreateMap<Role, RoleDto>()
            .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => src.Icon));

        CreateMap<Icons, IconDto>();

        CreateMap<UpdateUserRoleProgressDto, UserRoleProgress>();

        CreateMap<UserTechProgress, GetUserTechnologyProgressDto>()
            .ForMember(dest => dest.Tech, opt => opt.MapFrom(src => src.Tech));

        CreateMap<Technology, GetTechnologyDto>()
            .ForMember(dest => dest.IconPath, opt => opt.MapFrom(src => src.Icon != null ? src.Icon.Path : null));

        CreateMap<CreateUserTechnologyProgressDto, UserTechProgress>();

        CreateMap<CreateUserTechnologyProgressDto, UserTechProgress>();
        CreateMap<UpdateUserTechnologyProgressDto, UserTechProgress>();

        CreateMap<Icons, IconDto>();

        CreateMap<Technology, TechnologyDto>();

        CreateMap<Project, GetProjectDto>()
            .ForMember(dest => dest.Technologies, opt => opt.MapFrom(src => src.Technologies))
            .ForMember(dest => dest.Config, opt => opt.MapFrom(src => src.Config));
        
        CreateMap<User, GetUserDto>()
            .ForMember(dest => dest.ConfigJson, opt => opt.Ignore());
    }

}