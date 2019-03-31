using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DefaultEcs;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using Meltdown.Input;
using Meltdown.Utilities;

namespace Meltdown.Components
{
    //Create a new InputHandler class implementing the interface IInputHandler
    //Attach InputComponent initialized with the newly created InputHandler to an entity

    class InputComponent
    {
        IInputHandler inputHandler;

        public InputComponent(IInputHandler inputHandler)
        {
            this.inputHandler = inputHandler;
        }

        public void HandleInput(InputManager inputManager, Time time, Entity entity)
        {
            inputHandler.HandleInput(inputManager, time, entity);    
        }
    }

    interface IInputHandler
    {
       void HandleInput(InputManager inputManager, Time time, Entity entity); // TODO: add time and entity
    }
    
}
