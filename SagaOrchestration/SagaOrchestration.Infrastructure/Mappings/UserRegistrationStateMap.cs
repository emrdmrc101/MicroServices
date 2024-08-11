using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SagaOrchestration.Domain.State;

namespace SagaOrchestration.Infrastructure.Mappings;

public class UserRegistrationStateMap :SagaClassMap<UserRegistrationSagaState>
{
     protected override void Configure(EntityTypeBuilder<UserRegistrationSagaState> entity, ModelBuilder model)
     {
          entity.ToTable("UserRegistrationStates");
          entity.HasKey("CorrelationId");
          entity.Property(x => x.CorrelationId);
          entity.Property(x => x.CurrentState).HasMaxLength(50);
          entity.Property(x => x.UserId);
     }
}