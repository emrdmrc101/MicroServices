using System.ComponentModel.DataAnnotations;

namespace Lesson.Domain.Entities;

public class Lesson : BaseEntity
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    public string Description { get; set; }

    public DateTimeOffset? EndDate { get; set; }
    
    public UserLesson? UserLessons { get; set; }
}