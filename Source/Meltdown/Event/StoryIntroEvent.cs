using System;
using System.Diagnostics;

using DefaultEcs;

namespace Meltdown.Event
{
    class StoryIntroEvent : Event
    {
        public override void Initialize(World world)
        {
            Debug.WriteLine("INITIALIZE STORY INTRO");
        }

        public override void Update(World world)
        {
            Debug.WriteLine("UPDATE STORY INTRO");
        }
    }
}
