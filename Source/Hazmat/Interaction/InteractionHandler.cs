using System;
using System.Collections.Generic;

using DefaultEcs;

using Hazmat.Input;

namespace Hazmat.Interaction
{
    abstract class InteractionHandler
    {
        public bool commutative;
        EntitySet interacterSet;
        EntitySet interacteeSet;

        public InteractionHandler(EntitySet interacters, EntitySet interactees, bool commutative = false)
        {
            this.interacterSet = interacters;
            this.interacteeSet = interactees;
            this.commutative = commutative;
        }

        public bool HandleInteractions(IInputEvent inputEvent, Entity interacter, Entity interactee)
        {
            var interacters = this.interacterSet.GetEntities();
            var interactees = this.interacteeSet.GetEntities();

            if (interacters.IndexOf(interacter) != -1 && interactees.IndexOf(interactee) != -1)
            {
                if (this.HandleInteraction(inputEvent, interacter, interactee))
                {
                    return true;
                }
            }

            if (this.commutative && interacters.IndexOf(interactee) != -1 && interactees.IndexOf(interacter) != -1)
            {
                if (this.HandleInteraction(inputEvent, interacter, interactee))
                {
                    return true;
                }
            }

            return false;
        }

        public abstract bool HandleInteraction(IInputEvent inputEvent, Entity interactor, Entity interactee);
    }
}
