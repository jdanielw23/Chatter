using Chatter.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chatter
{
    public class AppDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> contextOptions) : base(contextOptions)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friendship>()
                .HasKey(e => new { e.UserId1, e.UserId2 });

            modelBuilder.Entity<Friendship>()
                .HasOne(e => e.User1)
                .WithMany(e => e.FriendsTo)
                .HasForeignKey(e => e.UserId1);

            modelBuilder.Entity<Friendship>()
                .HasOne(e => e.User2)
                .WithMany(e => e.FriendsFrom)
                .HasForeignKey(e => e.UserId2);
        }
    }
}