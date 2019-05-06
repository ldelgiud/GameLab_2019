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

        public bool IsEnabled { get; set; } = false;


        public EnergyEventSystem(Energy energy, Score score)
        {
            this.energy = energy;
            this.score = score;
        }

        public void Update(Time time)
        {
            if (energy.CurrentEnergy <= 0)
            {
                this.score.Complete(time);
                Hazmat.Instance.ActiveState.stateTransition = new SwapStateTransition(new ScoreState(this.score));
            }
        }

        public void Dispose()
        {

        }
    }
}
