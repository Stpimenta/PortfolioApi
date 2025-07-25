using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Domain.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUserDto, User>();
    }
}