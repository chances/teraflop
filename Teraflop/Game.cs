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
    public abstract class Game
    {
        private AssetDataLoader _assetDataLoader;
        private Renderer _renderer;
        protected FramebufferSizeProvider _framebufferSizeProvider;

        protected Game(int width = 800, int height = 600, string title = "Teraflop Game", bool fullscreen = false)
        {
            World = new World();
            LimitFrameRate = true;
            DesiredFramesPerSecond = 60.0;

            OpenTkWindow = new GameWindow(width, height, GraphicsMode.Default, title,
                fullscreen ? GameWindowFlags.Fullscreen : GameWindowFlags.Default,
                DisplayDevice.Default, 2, 0, GraphicsContextFlags.Debug);
        }

        private GameTime _gameTime;

        protected World World { get; }

        public bool IsActive { get; private set; }
        public bool LimitFrameRate { get; }
        public double DesiredFramesPerSecond { get; protected set; }
        public double FramesPerSecond => Math.Floor(OpenTkWindow?.RenderFrequency ?? 0);
        public MouseState MouseState { get; private set; }
        public KeyboardState KeyboardState { get; private set; }
        public string Title {
            get => OpenTkWindow?.Title ?? null;
            protected set
            {
                if (OpenTkWindow != null)
                {
                    OpenTkWindow.Title = value;
                }
            }
        }

        protected GameWindow OpenTkWindow { get; }

        public Dictionary<AssetType, string> AssetDirectoryPaths { get; } = new Dictionary<AssetType, string>();

        private TimeSpan TotalElapsedTime => _gameTime?.TotalGameTime ?? TimeSpan.Zero;

        protected Components.Camera Camera => World
            .FirstOrDefault(entity => entity.HasComponent<Components.Camera>())
            ?.GetComponent<Components.Camera>() ?? null;

        protected virtual void Initialize()
        {
            _assetDataLoader = new AssetDataLoader(Assembly.GetCallingAssembly(), AssetDirectoryPaths);

            InitializeWorld();
        }

        /// <summary>
        /// Initialize all world resources
        /// </summary>
        private void InitializeWorld()
        {
            // TODO: Get the DIP size from the _actual_ backing framebuffer
            _framebufferSizeProvider = new FramebufferSizeProvider(
                World, (uint) OpenTkWindow.Width, (uint) OpenTkWindow.Height
            );
            _renderer = new Renderer(World);
            new ComponentAssetLoader(World, _assetDataLoader).Operate();
            new ResourceInitializer(World).Operate();
        }

        public void Run()
        {
            _gameTime = new GameTime();
            OpenTkWindow.Load += (object sender, EventArgs e) =>
            {
                IsActive = true;

                Initialize();

                Update(_gameTime);

                // Set default values for clearing buffers
                GL.ClearColor(Color4.Black);
                GL.ClearDepth(1f);
                GL.ClearStencil(0);
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

                OpenTkWindow.ProcessEvents();
            };
            OpenTkWindow.RenderFrame += (object sender, FrameEventArgs e) =>
            {
                // TODO: Do I need to update game time here?
                Render(_gameTime);
            };

            OpenTkWindow.Run(DesiredFramesPerSecond, DesiredFramesPerSecond);
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
            new ComponentAssetLoader(World, _assetDataLoader).Operate();
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
            OpenTkWindow.Exit();
        }
    }
}
