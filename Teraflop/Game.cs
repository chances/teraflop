using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Teraflop.Assets;
using Teraflop.ECS;
using Teraflop.Input;
using Teraflop.Systems;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES20;

namespace Teraflop
{
    public abstract class Game : IDisposable
    {
        private readonly AssetDataLoader _assetDataLoader;
        private Renderer _renderer;
        protected FramebufferSizeProvider _framebufferSizeProvider;

        protected Game(int width = 800, int height = 600, string title = "Teraflop Game", bool fullscreen = false)
        {
            World = new World();
            LimitFrameRate = true;
            DesiredFramesPerSecond = 60.0;

            _assetDataLoader = new AssetDataLoader(Assembly.GetCallingAssembly(), AssetDirectoryPaths);

            OpenTkWindow = new GameWindow(width, height, GraphicsMode.Default, title,
                fullscreen ? GameWindowFlags.Fullscreen : GameWindowFlags.Default);
        }

        private GameTime _gameTime;

        protected World World { get; }

        public bool IsActive { get; private set; }
        public bool LimitFrameRate { get; }
        public double DesiredFramesPerSecond { get; protected set; }
        public double FramesPerSecond => Math.Round(OpenTkWindow?.RenderFrequency ?? 0, 2);
        public MouseState MouseState { get; private set; }
        public KeyboardState KeyboardState { get; private set; }

        // public GraphicsDevice GraphicsDevice { get; private set; }
        // public ResourceFactory ResourceFactory => GraphicsDevice.ResourceFactory;
        // public Framebuffer Framebuffer => GraphicsDevice.SwapchainFramebuffer;
        protected GameWindow OpenTkWindow { get; }

        public Dictionary<AssetType, string> AssetDirectoryPaths { get; } = new Dictionary<AssetType, string>();

        private TimeSpan TotalElapsedTime => _gameTime?.TotalGameTime ?? TimeSpan.Zero;

        protected Components.Camera Camera => World
            .FirstOrDefault(entity => entity.HasComponent<Components.Camera>())
            ?.GetComponent<Components.Camera>() ?? null;

        protected void Initialize()
        {
            // TODO: Get the DIP size from the _actual_ backing framebuffer
            _framebufferSizeProvider = new FramebufferSizeProvider(
                World, (uint) OpenTkWindow.Width, (uint) OpenTkWindow.Height
            );
            _renderer = new Renderer(World);
        }

        /// <summary>
        /// Initialize all world resources
        /// </summary>
        private void InitializeWorld()
        {
            new ComponentAssetLoader(World, _assetDataLoader).Operate();
            new ResourceInitializer(World).Operate();
        }

        public void Run()
        {
            _gameTime = new GameTime();
            OpenTkWindow.Run(60.0, DesiredFramesPerSecond);
            OpenTkWindow.Load += (object sender, EventArgs e) =>
            {
                IsActive = true;

                Initialize();
                InitializeWorld();

                Update(_gameTime);
                GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            };
            OpenTkWindow.Unload += (object sender, EventArgs e) =>
            {
                OpenTkWindow.MakeCurrent();
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

                // Dispose all world resources
                new ResourceDisposal(World).Operate();
            };
            OpenTkWindow.Resize += (object sender, EventArgs e) =>
            {
                // TODO: Get the DIP size from the _actual_ backing framebuffer
                _framebufferSizeProvider.Update(OpenTkWindow.Width, OpenTkWindow.Height);

                OpenTkWindow.MakeCurrent();
                GL.Viewport(0, 0, OpenTkWindow.Width, OpenTkWindow.Height);
            };
            OpenTkWindow.UpdateFrame += (object sender, FrameEventArgs e) =>
            {
                var delta = TimeSpan.FromSeconds(e.Time);
                _gameTime = new GameTime(TotalElapsedTime + delta, delta);
                if (OpenTkWindow.TargetRenderFrequency != DesiredFramesPerSecond)
                {
                    OpenTkWindow.TargetRenderFrequency = DesiredFramesPerSecond;
                }
                Update(_gameTime);
                if (!IsActive) OpenTkWindow.Exit();

                OpenTkWindow.ProcessEvents();
            };
            OpenTkWindow.RenderFrame += (object sender, FrameEventArgs e) =>
            {
                // TODO: Do I need to update game time here?
                Render(_gameTime);
            };
        }

        protected void ProcessInput(MouseState mouseState, KeyboardState keyboardState)
        {
            MouseState = mouseState;
            KeyboardState = keyboardState;
            // TODO: update input provider system for input receivers
        }

        protected virtual void Update(GameTime gameTime)
        {
            OpenTkWindow.MakeCurrent();

            new KeyboardProvider(World, KeyboardState).Operate();
            new ModelTransformationProvider(World).Operate();
            new ViewProjectionProvider(World, Camera?.ViewProjectionUniform ?? null).Operate();
            new ResourceInitializer(World).Operate();
            _framebufferSizeProvider.Operate();

            new ComponentUpdater(World).Operate(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected virtual void Render(GameTime gameTime)
        {
            OpenTkWindow.MakeCurrent();
            _renderer.Operate();
            OpenTkWindow.Context.SwapBuffers();
        }

        protected void Exit()
        {
            IsActive = false;
        }

        public abstract void Dispose();
    }
}
