using System;

using DefaultEcs;
using DefaultEcs.System;

using Hazmat.State;
using Hazmat.States;
using Hazmat.Utilities;

namespace Hazmat.Components
{
    class EnergyEventSystem : ISystem<Time>, IDisposable
    {
        Energy energy;
        Score score;
        PowerPlant plant;
        World world;

        public bool IsEnabled { get; set; } = false;


        public EnergyEventSystem(Energy energy, Score score, PowerPlant plant, World world)
        {
            this.energy = energy;
            this.score = score;
            this.plant = plant;
            this.world = world;
        }

        public void Update(Time time)
        {
            if (energy.CurrentEnergy <= 0)
            {
                this.score.Complete(time, false);
                EntitySet players = world.GetEntities().With<PlayerComponent>().Build();
                Hazmat.Instance.ActiveState.stateTransition = new SwapStateTransition(new ScoreState(this.score, this.plant, players));
            }
        }

        public void Dispose()
        {

        }
    }
}
