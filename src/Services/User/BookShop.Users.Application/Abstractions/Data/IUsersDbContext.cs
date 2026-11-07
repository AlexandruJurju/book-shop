using BookShop.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Users.Application.Abstractions.Data;

public interface IUsersDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Role> Roles { get; set; }
}
