using Jwt.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jwt.Context
{
    public class JwtDbContext : DbContext
    {
        public JwtDbContext(DbContextOptions<JwtDbContext> options) : base(options)
        {
        }
        public DbSet<UserEntity> Users { get; set; }
   
    }
}
