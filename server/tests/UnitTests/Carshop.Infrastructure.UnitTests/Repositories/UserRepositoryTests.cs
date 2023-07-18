using System;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Carshop.Domain.Enums;
using Carshop.Domain.Models;
using Carshop.Infrastructure.Context;
using Carshop.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Carshop.Infrastructure.UnitTests.Repositories;

public class UserRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task GetAll_ReturnsAllUsers()
    {
        // Arrange
        var users = new Faker<User>()
            .RuleFor(u => u.Id, f => f.Random.Guid())
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.FirstName, f => f.Person.FirstName)
            .RuleFor(u => u.LastName, f => f.Person.LastName)
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .Generate(3);

        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();

        var repository = new UserRepository(_context);

        // Act
        var result = await repository.GetAll();
        var resultList = result.ToList();

        // Assert
        resultList.Count.Should().Be(3);

        foreach (var user in users)
        {
            resultList.Should().Contain(u => user.Email == u.Email);
        }
    }

    [Fact]
    public async Task GetById_ShouldReturnUser_WhenValidIdIsProvided()
    {
        // Arrange
        Guid id;
        var user = new User
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Password = "password",
            Role = UserRoles.Admin.ToString()
        };

        var created = await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        id = created.Entity.Id;

        var repository = new UserRepository(_context);

        // Act
        var result = await repository.GetById(id);
        var count = await _context.Users.CountAsync();

        // Assert
        count.Should().Be(1);
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Email.Should().Be("test@example.com");
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.Password.Should().Be("password");
        result.Role.Should().Be(UserRoles.Admin.ToString());
    }

    [Fact]
    public async Task Save_Should_AddUserToDatabaseAndReturnCreatedEntity()
    {
        // Arrange
        var repository = new UserRepository(_context);

        // Create a user entity using Bogus library
        var user = new Faker<User>()
            .RuleFor(u => u.Id, f => f.Random.Guid())
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.FirstName, f => f.Person.FirstName)
            .RuleFor(u => u.LastName, f => f.Person.LastName)
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .Generate();

        // Act
        var createdUser = await repository.Save(user);

        // Assert
        _context.Users.Should().Contain(user);
        createdUser.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task GetByEmail_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var user = new User
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Password = "password",
            Role = UserRoles.Admin.ToString()
        };

        await _context.Set<User>().AddAsync(user);
        await _context.SaveChangesAsync();

        var repository = new UserRepository(_context);

        // Act
        var result = await repository.GetByEmail("test@example.com");

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("test@example.com");
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.Password.Should().Be("password");
        result.Role.Should().Be(UserRoles.Admin.ToString());
    }
}