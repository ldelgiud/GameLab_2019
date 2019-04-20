using Hazmat.Event;

namespace Hazmat.Components
{
    struct EventTriggerComponent
    {
        public Event.Event _event;

        public EventTriggerComponent(Event.Event _event)
        {
            this._event = _event;
        }
    }
}
