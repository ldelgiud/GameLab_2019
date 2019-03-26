﻿using System;

using DefaultEcs;

namespace Meltdown.Event
{
    public abstract class Event
    {
        public virtual void Initialize(World world, Entity entity) { }

        public abstract void Update(World world);
    }
}