namespace Infrastructure.Events
{
    public class EventSubscriber : IEventSubscriber
    {
        public EventSubscriber(IEventPublisher publisher)
        {
            publisher.ChangeEvent += HandleEventForChange;
            publisher.HideEvent += HandleEventForHide;
        }

        private void HandleEventForChange(string eventData)
        {
            // Send the new data somewhere :)
        }

        private void HandleEventForHide(IEnumerable<int> ids)
        {
            // Send the ids of the elements that need to be hide
        }
    }
}
