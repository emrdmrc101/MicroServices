namespace Shared.Objects.Identity;

public class IdentityContext
{
    public Guid UserId { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
}