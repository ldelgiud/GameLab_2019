using System;

using Meltdown.Event;

namespace Meltdown.Components
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
