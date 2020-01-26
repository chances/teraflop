using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Teraflop.Assets;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace Teraflop.Examples
{
    internal abstract class ExampleGame : Game
    {
        private Sdl2Window _window;

        public ExampleGame()
        {
            foreach (var resourceName in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                Console.WriteLine(resourceName);
            }
        }

        public string Title { get; set; } = "Example";
        public bool DebugMode { get; set; } = true;
        public static TinyIoC.TinyIoCContainer Services => TinyIoC.TinyIoCContainer.Current;

        protected override GraphicsDevice CreateGraphicsDevice()
        {
            // TODO: Migrate this to Sdl2Native.SDL_WINDOWPOS_CENTERED
            const int windowPositionCentered = 0x2FFF0000;
            var windowCreateInfo = new WindowCreateInfo
            {
                X = windowPositionCentered,
                Y = windowPositionCentered,
                WindowWidth = 960,
                WindowHeight = 540,
                WindowTitle = "Utility Grid"
            };
            _window = VeldridStartup.CreateWindow(ref windowCreateInfo);
            _window.Resized += () => _framebufferSizeProvider.Update(_window.Width, _window.Height);
            // _window.Closing += Exit; // TODO: Save game infrastructure
            _window.Closed += Exit;
            _window.CursorVisible = true;

            // TODO: Setup multisampling AA

            var options = new GraphicsDeviceOptions(DebugMode)
            {
                SwapchainDepthFormat = PixelFormat.R16_UNorm,
                PreferDepthRangeZeroToOne = true,
                PreferStandardClipSpaceYDirection = true
            };

            var isWindowsOrMacOs = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var defaultBackend = VeldridStartup.GetPlatformDefaultBackend();
            var device = isWindowsOrMacOs
                ? VeldridStartup.CreateGraphicsDevice(_window, options)
                : defaultBackend == GraphicsBackend.Vulkan
                    ? VeldridStartup.CreateVulkanGraphicsDevice(options, _window)
                    : VeldridStartup.CreateDefaultOpenGLGraphicsDevice(options, _window, defaultBackend);

            Services.Register<BufferFactory>(new BufferFactory(device.ResourceFactory));

            return device;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!_window.Exists)
            {
                Exit();
                return;
            }

            var inputSnapshot = _window.PumpEvents();
            ProcessInput(InputSnapshotConverter.Mouse(inputSnapshot, MouseState),
                InputSnapshotConverter.Keyboard(inputSnapshot));

            if (KeyboardState.IsKeyDown(Input.Key.Escape))
                Exit();

            base.Update(gameTime);

            if (DebugMode) {
                var frameTime = Math.Round(gameTime.ElapsedGameTime.TotalMilliseconds, 1);
                _window.Title = $"{Title} - {frameTime} ms - {FramesPerSecond} fps";
            }
        }
    }
}
