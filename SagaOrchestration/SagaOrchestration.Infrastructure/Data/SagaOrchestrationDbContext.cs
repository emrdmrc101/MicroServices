using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using SagaOrchestration.Domain.State;
using SagaOrchestration.Infrastructure.Mappings;

namespace SagaOrchestration.Infrastructure.Data;

public class SagaOrchestrationDbContext : SagaDbContext
{ 
    public DbSet<UserRegistrationSagaState> UserRegistrationStates { get; set; }
    public SagaOrchestrationDbContext(DbContextOptions options) : base(options)
    {
    }
    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new UserRegistrationStateMap(); }
    }
}