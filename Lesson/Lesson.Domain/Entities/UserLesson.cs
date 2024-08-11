using System.ComponentModel.DataAnnotations;

namespace Lesson.Domain.Entities;

public class UserLesson : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    
    public Guid LessonId { get; set; }

    public Lesson Lesson { get; set; }
}