namespace Identity.Domain.Commands;

public class UserRegisterCommand
{
    public Guid CorrelationId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}