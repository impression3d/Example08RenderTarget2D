using Impression;
using Impression.Graphics;
using Impression.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Example08RenderTarget2D
{
    public class Example08RenderTarget2DGame : Impression.Game
    {
		GraphicsDeviceManager graphics;

		Camera mainCamera;
		Camera secondaryCamera;

		RenderTarget2D renderTarget;

		SpriteBatch spriteBatch;

		Model model;

		Matrix modelTransform;
        float rotationSpeed = 1;

        public Example08RenderTarget2DGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);

			switch (FrameworkContext.Platform)
			{
				case PlatformType.Windows:
				case PlatformType.Mac:
				case PlatformType.Linux:
					{
						graphics.PreferredBackBufferWidth = 1280;
						graphics.PreferredBackBufferHeight = 720;

						this.IsMouseVisible = true;
					}
					break;
                case PlatformType.WindowsStore:
                case PlatformType.WindowsMobile:
					{
						graphics.PreferredBackBufferWidth = 1280;
						graphics.PreferredBackBufferHeight = 720;

						graphics.IsFullScreen = true;

						// Frame rate is 30 fps by default for mobile device
						TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 30L);
					}
					break;
				case PlatformType.Android:
				case PlatformType.iOS:
					{
						graphics.PreferredBackBufferWidth = 1280;
						graphics.PreferredBackBufferHeight = 720;

						graphics.IsFullScreen = true;

						// Frame rate is 30 fps by default for mobile device
						TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 30L);
					}
					break;
			}

            this.View.Title = "Example08RenderTarget2D";
        }

        protected override void Initialize()
        { 
            base.Initialize();

            // Down side to 1/4 of screen viewport
			renderTarget = new RenderTarget2D(graphics.GraphicsDevice, graphics.GraphicsDevice.Viewport.Width / 4, graphics.GraphicsDevice.Viewport.Height / 4, false, SurfaceFormat.Color, DepthFormat.Depth16);

            mainCamera = new Camera(graphics.GraphicsDevice.Viewport);
			mainCamera.Transform = Matrix.CreateTranslation(0, 2f, 10);

			secondaryCamera = new Camera(new Viewport(0, 0, graphics.GraphicsDevice.Viewport.Width / 4, graphics.GraphicsDevice.Viewport.Height / 4));
			secondaryCamera.Transform = Matrix.CreateRotationY(MathHelper.ToRadians(180)) * Matrix.CreateTranslation(0, 2, -10);

			spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            model = this.Content.Load<Model>("Models/Tank/tank");
        }

        protected override void UnloadContent()
        {
            // do something

            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
			switch (FrameworkContext.Platform)
			{
				case PlatformType.Windows:
				case PlatformType.Mac:
				case PlatformType.Linux:
					{
						if (Keyboard.GetState().IsKeyDown(Keys.Escape))
							this.Exit();
					}
					break;
				default:
					{
						// do nothings
					}
					break;
			}

			modelTransform = Matrix.CreateScale(Vector3.One) *
								   Matrix.CreateRotationY((float)gameTime.TotalGameTime.TotalSeconds * rotationSpeed) *
								   Matrix.CreateTranslation(Vector3.Zero);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
			graphics.GraphicsDevice.Clear(Color.BurlyWood);

			// Set render target
			graphics.GraphicsDevice.SetRenderTarget(renderTarget);

			graphics.GraphicsDevice.Clear(Color.Aqua);

			// Draw model using secondary camera on render target
			model.Draw(modelTransform, secondaryCamera.View, secondaryCamera.Projection);

			// Unset render target
			graphics.GraphicsDevice.SetRenderTarget(null);

			graphics.GraphicsDevice.Clear(Color.BurlyWood);

			// Draw model using main camera on back buffer
			model.Draw(modelTransform, mainCamera.View, mainCamera.Projection);

			spriteBatch.Begin();

			// Draw render target on right bottom screen
            spriteBatch.Draw(
                renderTarget,
                new Vector2(
                    graphics.GraphicsDevice.Viewport.Width - 16, 
                    graphics.GraphicsDevice.Viewport.Height - 16),
                null,
                Color.White,
                0,
                new Vector2(renderTarget.Width, renderTarget.Height),
                1,
                SpriteEffects.None,
                0);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}