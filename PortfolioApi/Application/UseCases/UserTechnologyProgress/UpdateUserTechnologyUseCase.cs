using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.Application.UseCases.UserTechnologyProgress;

public class UpdateUserTechnologyUseCase
{
    private readonly IUserTechnologyRepository _repository;
    private readonly IMapper _mapper;

    public UpdateUserTechnologyUseCase(IUserTechnologyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task ExecuteAsync(UpdateUserTechnologyProgressDto dto)
    {
        var exists = await _repository.ExistsAsync(dto.UserId, dto.TechId);
        if (!exists)
            throw new BusinessException("User technology progress not found for update.");

        var userTechProgress = _mapper.Map<Domain.Entities.UserTechProgress>(dto);
        await _repository.UpdateAsync(userTechProgress);
    }
}