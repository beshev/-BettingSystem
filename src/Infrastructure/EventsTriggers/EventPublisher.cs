namespace Infrastructure.Events
{
    using Newtonsoft.Json;

    public class EventPublisher : IEventPublisher
    {
        public delegate void EventHandlerChange(string eventData);

        public delegate void EventHandlerHide(int id);

        public event EventHandlerChange ChangeEvent;

        public event EventHandlerHide HideEvent;

        public void TriggerEventForChanges<T>(T eventData)
        {
            var eventDataAsString = JsonConvert.SerializeObject(eventData);
            if (ChangeEvent != null)
            {
                ChangeEvent.Invoke(eventDataAsString);
            }
        }

        public void TriggerEventForHide(int id)
        {
            if (ChangeEvent != null)
            {
                HideEvent.Invoke(id);
            }
        }
    }
}
