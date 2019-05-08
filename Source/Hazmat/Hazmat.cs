using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Hazmat.State;
using Hazmat.States;
using Hazmat.ResourceManagers;
using Hazmat.Utilities;
using Hazmat.Music;

namespace Hazmat
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Hazmat : Game
    {
        public static Hazmat Instance { get; private set; }

        private Stack<State.State> stateStack = new Stack<State.State>();

        public State.State ActiveState { get { return this.stateStack.Peek(); } }

        Time updateTime;
        Time drawTime;

        GraphicsDeviceManager graphics;

        internal AtlasTextureResourceManager atlasTextureResourceManager;
        internal ModelResourceManager modelResourceManager;
        internal SpineAnimationResourceManager spineAnimationResourceManager;
        internal TextResourceManager textResourceManager;
        internal TextureResourceManager textureResourceManager;
        internal TileModelResourceManager tileModelResourceManager;

        public SoundManager SoundManager { get; private set; }

        public Hazmat()
        {
            Hazmat.Instance = this;
          
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            SoundManager = new SoundManager(Content);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Disable B button as back on Xbox
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (sender, ev) => { ev.Handled = true; };

            this.updateTime = new Time();
            this.drawTime = new Time();

            this.IsMouseVisible = true; // it is fucking visible OK!

            this.atlasTextureResourceManager = new AtlasTextureResourceManager(this.GraphicsDevice, @"items\SPS_StaticSprites");
            this.modelResourceManager = new ModelResourceManager(this.Content);
            this.spineAnimationResourceManager = new SpineAnimationResourceManager(this.GraphicsDevice);
            this.textResourceManager = new TextResourceManager(this.Content);
            this.textureResourceManager = new TextureResourceManager(this.Content);
            this.tileModelResourceManager = new TileModelResourceManager(this.GraphicsDevice, @"items\SPS_StaticSprites");

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            var initialState = new CoverState();
            this.stateStack.Push(initialState);

            // Resource Preloading
            this.spineAnimationResourceManager.Load(@"ui\SPS_Screens");
            this.spineAnimationResourceManager.Load(@"items\SPS_Collectables");
            this.spineAnimationResourceManager.Load(@"items\SPS_Projectiles");
            this.spineAnimationResourceManager.Load(@"items\SPS_StaticSprites");

            // Time is null for cover state, shouldn't cause any problems
            initialState.Initialize(null, this);
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
            this.updateTime.Update(gameTime);
            IStateTransition transition = this.ActiveState.Update(this.updateTime);
            switch (transition)
            {
                case PopStateTransition t:
                    // Destroy current state
                    this.ActiveState.Destroy();

                    // Remove from stack
                    this.stateStack.Pop();

                    // Resume top state
                    this.ActiveState.Resume(t.Data);
                    break;
                case SwapStateTransition t:
                    // Destroy current state
                    this.ActiveState.Destroy();

                    // Remove from stack
                    this.stateStack.Pop();

                    // Add to stack
                    this.stateStack.Push(t.State);

                    // Initialize new state
                    t.State.Initialize(updateTime, this);

                    break;
                case PushStateTransition t:
                    // Suspend current state
                    this.ActiveState.Suspend();

                    // Add to stack
                    this.stateStack.Push(t.State);

                    // Initialize new state
                    t.State.Initialize(updateTime, this);
                   
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
            this.drawTime.Update(gameTime);
            GraphicsDevice.Clear(Color.Black);
            this.ActiveState.Draw(this.drawTime);
            base.Draw(gameTime);
        }
        
    }
}
