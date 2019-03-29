using Meltdown.Event;

namespace Meltdown.Components
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
