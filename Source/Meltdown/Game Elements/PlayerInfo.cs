﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Meltdown.Components;
using Microsoft.Xna.Framework;

namespace Meltdown.Game_Elements
{
    class PlayerInfo
    {
        Vector2 position;
        
        public void Update(Vector2 position)
        {
            this.position = position;

        }
    }
}
