using Microsoft.EntityFrameworkCore;

namespace Lesson.Infrastructure.Data;

public class LessonDbContext(DbContextOptions<LessonDbContext> options) : DbContext(options)
{
    public DbSet<Domain.Entities.Lesson> Lessons { get; set; }
    public DbSet<Domain.Entities.UserLesson> UserLessons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.Lesson>()
            .HasKey(x => x.Id);
        modelBuilder.Entity<Domain.Entities.UserLesson>()
            .HasKey(x => x.Id);

        base.OnModelCreating(modelBuilder);
    }
}