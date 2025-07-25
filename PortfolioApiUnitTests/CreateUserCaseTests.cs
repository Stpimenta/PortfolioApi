using AutoMapper;
using Xunit;
using Moq;
using PortfolioApi.Application.UseCases.Users;
using PortfolioApi.Domain.Entities;
using PortfolioApi.Application.Dtos;
using PortfolioApi.Application.Services;
using PortfolioApi.Infrastructure.Repository.Interfaces;
using PortfolioApi.Shared.Exceptions;

public class CreateUserUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_WithValidUser_ShouldReturnUserId()
    {
       
        var mockRepo = new Mock<IUserRepository>();
        var mockPassword = new Mock<IPasswordService>();
        var mockMapper = new Mock<IMapper>();

        var useCase = new AddUserUseCase(mockRepo.Object, mockMapper.Object, mockPassword.Object);

        var dto = new CreateUserDto 
        { 
            Name = "Test User",
            Email = "test@example.com", 
            Password = "123456" 
        };

     
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = "hashed123"
        };

        mockRepo.Setup(r => r.GetUserByEmail(dto.Email)).ReturnsAsync((User?)null);
        mockPassword.Setup(p => p.HashPassword(dto.Password)).Returns("hashed123");
        mockMapper.Setup(m => m.Map<User>(dto)).Returns(user);
        mockRepo.Setup(r => r.AddUserAsync(user)).ReturnsAsync(1);

   
        var result = await useCase.ExecuteAsync(dto);

        Assert.Equal(1, result);
    }
    
    [Fact]
    public async Task ExecuteAsync_WithExistingEmail_ShouldThrowBusinessException()
    {
        var mockRepo = new Mock<IUserRepository>();
        var mockPassword = new Mock<IPasswordService>();
        var mockMapper = new Mock<IMapper>();

        var useCase = new AddUserUseCase(mockRepo.Object, mockMapper.Object, mockPassword.Object);

        var dto = new CreateUserDto
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "123456"
        };

        var existingUser = new User { Email = dto.Email };
        
        mockRepo.Setup(r => r.GetUserByEmail(dto.Email)).ReturnsAsync(existingUser);
            
        var exception = await Assert.ThrowsAsync<BusinessException>(() => useCase.ExecuteAsync(dto));
        Assert.Equal("email already registered.", exception.Message);
    }
    
}