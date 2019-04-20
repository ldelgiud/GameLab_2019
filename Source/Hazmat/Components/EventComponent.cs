using System;

using Hazmat.Event;

namespace Hazmat.Components
{
    struct EventComponent
    {
        public Event.Event value;

        public EventComponent(Event.Event _event)
        {
            this.value = _event;
        }
    }
}
