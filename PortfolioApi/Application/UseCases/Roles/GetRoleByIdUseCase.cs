using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Domain.Entities;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.Roles;

public class GetRoleByIdUseCase
{
    private readonly IRoleRepository _repository;
    private readonly IMapper _mapper;

    public GetRoleByIdUseCase(IRoleRepository repository,  IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RoleWithTechnologiesDto> ExecuteAsync(int id)
    {
        var role = await _repository.GetByIdAsync(id);
        if (role == null)
            throw new NotFoundException($"Role with id {id} not found.");

        return _mapper.Map<RoleWithTechnologiesDto>(role);
    }
}
