using Infrastructure.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public partial class UserDataContext : DbContext
{
    public UserDataContext(DbContextOptions<UserDataContext> options) : base(options)
    {
    }

    protected UserDataContext()
    {
    }

    public virtual DbSet<RoleEntity> Roles { get; set; }
    public virtual DbSet<AddressEntity> Addresses { get; set; }
    public virtual DbSet<UserEntity> Users { get; set; }
    public virtual DbSet<ProfileEntity> Profiles { get; set; }
    public virtual DbSet<AuthenticationEntity> Authentications { get; set; }
}
