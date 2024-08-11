namespace Shared.Events.IdentityService.UserRegistration;

public class UserRegisteredEvent 
{
    public Guid CorrelationId { get; set; }
    public Guid UserId { get; set; }
}