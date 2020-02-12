using kviz_jatek.Model;
using Microsoft.EntityFrameworkCore;

namespace kviz_jatek.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<TopScore> TopScores { get; set; }

        public DbSet<QuizContent> QuizContents { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=kvizdatabase.db");
        }
    }
}
