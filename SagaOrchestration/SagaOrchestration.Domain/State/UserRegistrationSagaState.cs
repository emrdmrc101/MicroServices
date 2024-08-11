using System.ComponentModel.DataAnnotations;
using MassTransit;

namespace SagaOrchestration.Domain.State;

public class UserRegistrationSagaState : SagaStateMachineInstance
{
    [Key]
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid UserId { get; set; }
}