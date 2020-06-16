using MyAppBack.Identity;

namespace MyAppBack.Services.TokenService
{
  public interface ITokenService
  {
    string CreateToken(AppUser user);
  }
}