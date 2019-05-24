using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hazmat.State;
using Hazmat.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Resource;

using Hazmat.Input;
using Hazmat.Graphics;
using Hazmat.Components;
using Hazmat.ResourceManagers;
using Hazmat.Systems;
using Hazmat.Event;

namespace Hazmat.States
{
    class VideoState : State.State
    {
        GameWindow window;
        VideoPlayer player;
        InputManager inputManager;
        Camera2D screenCamera;
        World world;
        Texture2D videoTexture;
        ISystem<Time> drawSystem;
        float timeStamp;
        SpriteBatch spriteBatch;
        Entity entity;
       
        public override void Initialize(Time time, Hazmat game)
        {
            this.inputManager = new InputManager();
            this.inputManager.Register(Keys.Enter);
            this.inputManager.Register(Buttons.A);
            this.SetInstance(this.inputManager);

            this.window = game.Window;

            this.screenCamera = new Camera2D(
                new Transform2D(),
                1920,
                1080
                );

            // World
            this.world = new World();

            // Resource Managers
            Hazmat.Instance.spineAnimationResourceManager.Manage(this.world);

            // Systems
            this.drawSystem = new SequentialSystem<Time>(
                new AnimationStateUpdateSystem(this.world),
                new SkeletonUpdateSystem(this.world),
                new SpineSkeleton2DDrawSystem<ScreenSpaceComponent>(game.GraphicsDevice, this.screenCamera, this.world)
                );

            player = new Microsoft.Xna.Framework.Media.VideoPlayer();

            Video video = video = game.Content.Load<Video>("hazmat-intro");
            player.IsLooped = false;
            player.Play(video);
            videoTexture = null;
            timeStamp = time.Absolute + 62;


            spriteBatch = new SpriteBatch(game.GraphicsDevice);

        }

        public override IStateTransition Update(Time time)
        {
            this.inputManager.Update(time);

            IInputEvent inputEvent = this.inputManager.GetEvent(Keys.Enter) ?? this.inputManager.GetEvent(0, Buttons.A);

            if (player.State != MediaState.Stopped)
            {
                videoTexture = player.GetTexture();
            }
            if (videoTexture != null)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(videoTexture, new Rectangle(0, 0, 400, 240), Color.White);
                spriteBatch.End();
            }

            switch (inputEvent)
            {
                case ReleaseEvent _:
                    this.stateTransition = new SwapStateTransition(new MainMenuState());
                    player.Stop();
                    break;
            }

            if (time.Absolute >= timeStamp)
            {
                this.stateTransition = new SwapStateTransition(new MainMenuState());
                player.Stop();
            }

            return base.Update(time);

        }

        public override void Draw(Time time)
        {
            this.screenCamera.Update(this.window);
            this.drawSystem.Update(time);
        }
    }
}
