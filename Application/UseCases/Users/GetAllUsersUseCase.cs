using AutoMapper;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Domain.Entities;
using PortfolioApi.Infrastructure.Repository.Interfaces;

namespace PortfolioApi.Application.UseCases.Users;

public class GetAllUsersUseCase
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    public GetAllUsersUseCase(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetUserDto>> ExecuteAsync()
    {
        var users = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<GetUserDto>>(users);
    }


}