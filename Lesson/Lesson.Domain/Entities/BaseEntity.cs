using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lesson.Domain.Entities;

public class BaseEntity
{
    [Key]
    public DateTimeOffset CreationDate { get; set; }

    [Required]
    public Guid CreatorID { get; set; }

    [DefaultValue(false)]
    public bool Deleted { get; set; }
}