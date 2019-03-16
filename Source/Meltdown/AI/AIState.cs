using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Meltdown.Game_Elements;

namespace Meltdown.AI
{
    abstract class AIState
    {
        abstract public AIState UpdateState(List<PlayerInfo> playerInfos, Vector2 pos, ref Vector2 velocity);
    }
}
