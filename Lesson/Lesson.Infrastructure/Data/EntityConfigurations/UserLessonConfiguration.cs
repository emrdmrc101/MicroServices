using Lesson.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lesson.Infrastructure.Data.EntityConfigurations;

public class UserLessonConfiguration : IEntityTypeConfiguration<UserLesson>
{
    public void Configure(EntityTypeBuilder<UserLesson> builder)
    {
        builder.HasOne(x => x.Lesson)
            .WithOne(x => x.UserLessons)
            .HasForeignKey<UserLesson>(x => x.LessonId);
    }
}