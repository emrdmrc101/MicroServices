using System.ComponentModel.DataAnnotations;

namespace Identity.Domain.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string UserName { get; set; }
    
    [Required]
    [MaxLength(250)]
    public string Password { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Email { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
}