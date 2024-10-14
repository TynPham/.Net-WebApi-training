using WebApi.Model;

namespace WebApi.Interfaces;

public interface ITokenServices
{
    string CreateToken(AppUser user);
}