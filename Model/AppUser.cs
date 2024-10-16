using Microsoft.AspNetCore.Identity;

namespace WebApi.Model;

public class AppUser : IdentityUser
{
    public List<Portfolio> Portfolios  { get; set; } = new List<Portfolio>();
}