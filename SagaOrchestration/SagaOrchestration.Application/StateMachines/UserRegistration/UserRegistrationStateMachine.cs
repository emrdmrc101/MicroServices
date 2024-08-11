using MassTransit;
using SagaOrchestration.Domain.State;
using Shared.Events.IdentityService.UserRegistration;
using Shared.Events.LessonService.LessonAssignment;

namespace SagaOrchestration.Application.StateMachines.UserRegistration;

public class UserRegistrationStateMachine : MassTransitStateMachine<UserRegistrationSagaState>
{
    public State LessonAssignmentFailed { get; private set; }
    public State LessonAssignmentPending { get; private set; }
    public State Completed { get; private set; }

    public Event<UserRegisteredEvent> RegisteredEvent { get; private set; }
    public Event<LessonAssignedEvent> LessonsAssignedEvent { get; private set; }
    public Event<Fault<LessonAssignmentRequestedEvent>> LessonsAssignedFaultEvent { get; private set; }

    public UserRegistrationStateMachine()
    {
        InstanceState(x => x.CurrentState);
        
        /*
         * "CorrelateById" property için, saga sürecinde mesajların  hangi saga akışı ile eşleştirileceğini belirler.
         */
        Event(() => RegisteredEvent, x => x.CorrelateById(c => c.Message.UserId));
        Event(() => LessonsAssignedEvent, x => x.CorrelateById(c => c.Message.UserId));
        Event(() => LessonsAssignedFaultEvent, x => x.CorrelateById(c => c.Message.Message.UserId));

        /*
         * Akış başlangıç "RegisteredEvent" fırlatıldığında akış başlatılır.
         * "RegisteredEvent" içerisinden gelen UserId değeri saga içerisindeki state > UserId değerine atanır.
         * "LessonAssignmentRequestedEvent" ders atamalarının yapılması için fırlatılır. Bu event dışarıdanda tetiklenebilir.
         Biz örnek için buradan akışı direkt ilerletiyoruz.
         * TransitionTo ile saga state "LessonAssignmentPending durumuna geçer.
         */
        Initially(
            When(RegisteredEvent)
                .Then(context => { context.Saga.UserId = context.Message.UserId; })
                .ThenAsync(context => context.Publish(new LessonAssignmentRequestedEvent()
                {
                    UserId = context.Saga.UserId
                }))
                .TransitionTo(LessonAssignmentPending)
        );

        /*
         * Saga state "LessonAssignmentPending" ise ve "LessonsAssignedEvent" fırlatılmışsa "TransitionTo" ile
         saga state "Completed" durumuna set edilir.
         * "LessonsAssignedFaultEvent" fırlatılmışssa saga durumu "LessonAssignmentFailed" durumuna set edilir.
         "LessonsAssignedFaultEvent" için, consume içerisinde bir exception oluştuğunda masstransit Fault<T> eventi fırlatır.
         Biz bu eventı yakalıyarak bir exception olduğunu anlayıp durumu set ediyoruz.
         Gerekli geri alma vb işlemleri consume içerisinde Fault<T> eventi dinleyen handle da yapabiliriz.
         */
        During(LessonAssignmentPending,
            When(LessonsAssignedEvent)
                .TransitionTo(Completed),
            When(LessonsAssignedFaultEvent)
                .TransitionTo(LessonAssignmentFailed)
        );

        // Saga akışının artık tamamlandığını  başka herhangi bir  event vb takip etmeyeceğini belirtiyoruz.
        SetCompletedWhenFinalized();
    }
}