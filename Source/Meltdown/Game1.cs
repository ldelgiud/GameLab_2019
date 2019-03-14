﻿using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Meltdown.State;
using Meltdown.States;

namespace Meltdown
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static Game1 Instance { get; private set; }

        private Stack<IState> stateStack = new Stack<IState>();

        public IState ActiveState { get { return this.stateStack.Peek(); } }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            Game1.Instance = this;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            IState initialState = new MainMenuState();
            initialState.Initialize(this);
            this.stateStack.Push(initialState);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            IStateTransition transition = this.ActiveState.Update(gameTime);
            switch (transition)
            {
                case PopStateTransition t:
                    // Destroy current state
                    this.ActiveState.Destroy();

                    // Remove from stack
                    this.stateStack.Pop();

                    // Resume top state
                    this.ActiveState.Resume();
                    break;
                case SwapTransition t:
                    // Destroy current state
                    this.ActiveState.Destroy();

                    // Remove from stack
                    this.stateStack.Pop();

                    // Initialize new state
                    t.State.Initialize(this);

                    // Add to stack
                    this.stateStack.Push(t.State);
                    break;
                case PushStateTransition t:
                    // Suspend current state
                    this.ActiveState.Suspend();

                    // Initialize new state
                    t.State.Initialize(this);

                    // Add to stack
                    this.stateStack.Push(t.State);
                    break;
                case ExitTransition t:
                    // Exit game
                    this.Exit();
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            this.ActiveState.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
