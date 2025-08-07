using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.UserRoleProgress;

public class UpdateUserRoleProgressUseCase
{
    private readonly IUserRoleProgressRepository _repository;
    private readonly IMapper _mapper;
    
    public UpdateUserRoleProgressUseCase(
        IUserRoleProgressRepository repository,  
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task ExecuteAsync(UpdateUserRoleProgressDto dto)
    {
        var exist = await _repository.ExistsAsync(dto.UserId, dto.RoleId);
        if (!exist)
        {
            throw new BusinessException("User role progress not found for update");
        }
        
        var roleProgress = _mapper.Map<Domain.Entities.UserRoleProgress>(dto);
        await _repository.UpdateAsync(roleProgress);
    }
}