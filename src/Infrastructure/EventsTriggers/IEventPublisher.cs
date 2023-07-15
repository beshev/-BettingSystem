namespace Infrastructure.Events
{
    public interface IEventPublisher
    {
        event EventPublisher.EventHandlerChange ChangeEvent;

        event EventPublisher.EventHandlerHide HideEvent;

        void TriggerEventForChanges<T>(IEnumerable<T> eventData);

        void TriggerEventForHide(IEnumerable<int> ids);
    }
}