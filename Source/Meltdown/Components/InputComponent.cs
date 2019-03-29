using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DefaultEcs;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Meltdown.Components
{
    //Create a new InputHandler class implementing the interface IInputHandler
    //Attach InputComponent initialized with the newly created InputHandler to an entity

    class InputComponent
    {
        public InputComponent(IInputHandler inputHandler)
        {
            this.inputHandler = inputHandler;
        }

        IInputHandler inputHandler;
        public void HandleInput()
        {
            inputHandler.HandleInput();    
        }
    }

    interface IInputHandler
    {
       void HandleInput();
    }
    
}
