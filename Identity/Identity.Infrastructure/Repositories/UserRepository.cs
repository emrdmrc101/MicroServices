using Identity.Domain.Entities;
using Identity.Domain.Interfaces.Repositories;
using Identity.Infrastructure.Data;

namespace Identity.Infrastructure.Repositories;

public class UserRepository(IdentityDbContext context) : Repository<User>(context), IUserRepository
{
}