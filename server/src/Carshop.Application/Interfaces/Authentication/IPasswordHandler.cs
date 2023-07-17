namespace Carshop.Application.Interfaces.Authentication;

public interface IPasswordHandler
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}