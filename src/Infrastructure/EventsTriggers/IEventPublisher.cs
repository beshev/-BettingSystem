namespace Infrastructure.Events
{
    public interface IEventPublisher
    {
        event EventPublisher.EventHandlerChange ChangeEvent;

        event EventPublisher.EventHandlerHide HideEvent;

        void TriggerEventForChanges<T>(T eventData);

        void TriggerEventForHide(int id);
    }
}