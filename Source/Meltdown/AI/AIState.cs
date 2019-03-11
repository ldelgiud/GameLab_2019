using Meltdown.Game_Elements;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Meltdown.AI
{
    abstract class AIState
    {
        abstract public AIState UpdateState(List<PlayerInfo> playerInfos, Vector2 pos, ref Vector2 velocity);
    }
}
