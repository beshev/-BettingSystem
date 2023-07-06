namespace Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {}

        public DbSet<Sport> Sports { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<Bet> Bets { get; set; }

        public DbSet<Odd> Odds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
