using Lab5.Application.Models.Users;
using Lab5.Application.Services.Users;

namespace Lab5.Application.Users;

public class CurrentUserManager : ICurrentUserService
{
    public User? User { get; set; }
}
