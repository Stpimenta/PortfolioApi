using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Domain.Entities;
using PortfolioApi.Infrastructure.Repository.Interfaces;

namespace PortfolioApi.Application.UseCases.Roles;

public class GetAllRolesUseCase
{
    private readonly IRoleRepository _repository;
    private readonly IMapper _mapper;

    public GetAllRolesUseCase(IRoleRepository repository,  IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<RoleWithTechnologiesDto>> ExecuteAsync()
    {
        var roles = await _repository.GetAllAsync();
        return _mapper.Map<List<RoleWithTechnologiesDto>>(roles);
    }
}