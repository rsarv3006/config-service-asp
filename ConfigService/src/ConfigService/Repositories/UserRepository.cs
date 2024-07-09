using ConfigService.Models;
using Microsoft.EntityFrameworkCore;

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
  
  public async Task<User?> GetUser(string appName)
  {
    return await _context.Users.FirstOrDefaultAsync(u => u.AppName == appName);
  }
  
  public async Task<User?> GetUser(Guid userId)
  {
    return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
  }
}
