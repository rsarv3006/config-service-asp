using ConfigService.Models;

namespace ConfigService.Repositories;

public class UserRepository
{
  private readonly ConfigServiceContext _context;

  public UserRepository(ConfigServiceContext context)
  {
    _context = context;
  }

  public async Task<User?> CreateAdminUser()
  {
    var user = new User(UserRole.Admin, "admin");
    _context.Users.Add(user);
    await _context.SaveChangesAsync();
    return user;
  }

  public async Task<User?> CreateUser(string appName)
  {
    var user = new User(UserRole.User, appName);
    _context.Users.Add(user);
    await _context.SaveChangesAsync();
    return user;
  }
}
