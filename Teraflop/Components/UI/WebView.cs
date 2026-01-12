using SixLabors.Fonts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using Teraflop.Components.Receivers;
using Teraflop.ECS;
using UltralightNet;
using UltralightNet.Platform;
using Veldrid;

namespace Teraflop.Components.UI {
	// TODO: Use GPU accelerated rendering via Veldrid.
	// See https://github.com/SupinePandora43/UltralightNet/blob/b596b10aa60e0d7d8b216121e546434572a2df52/Examples/gpu/UltralightNet.Veldrid.TestApp/Program.cs#L22
	public class WebView : Resource, IFramebufferSize, IUpdatable {
		public event EventHandler TextureRecreated;

		private GraphicsDevice _device;
		private Texture _texture;
		private static readonly FontLoader _fontLoader = new();
		private static Renderer _renderer = null;
		private View _view;
		private Size _size = new(0, 0);

		public WebView() : base("WebView") {
			Resources.Initializing += (_, e) => {
				_device = e.GraphicsDevice;

				_size = new Size(
					(int)_device.SwapchainFramebuffer.Width,
					(int)_device.SwapchainFramebuffer.Height
				);

				_view = _renderer.CreateView(
					Convert.ToUInt32(_size.Width),
					Convert.ToUInt32(_size.Height),
					viewConfig: new ULViewConfig() {
						IsTransparent = true,
						EnableJavaScript = true,
					}
				);
				CreateTexture();
			};
			Resources.Disposed += (_, __) => {
				_view.Dispose();
				_texture.Dispose();
			};
		}

		public Size FramebufferSize {
			set {
				_size = value;

				// Recreate the UI's backing texture, sampler, and surface
				CreateTexture();
			}
		}

		public Texture Texture => _texture;

		public static void InitializeRenderer(string appName) {
			ULPlatform.SetDefaultFileSystem = true;
			UltralightNet.AppCore.AppCoreMethods.SetPlatformFontLoader();
			//ULPlatform.FontLoader = _fontLoader;
			_renderer = ULPlatform.CreateRenderer(new ULConfig() {
				CachePath = Path.Join(Path.GetTempPath(), appName, "Ultralight"),
			});
		}

		public unsafe void Update(GameTime gameTime) {
			_renderer.Update();
			_renderer.Render();

			// Blit the webview to the GPU
			var pixels = _view.Surface?.Bitmap ?? null;
			if (pixels != null) throw new Exception("Could not acquire web view viewport bitmap.");
			var width = Convert.ToUInt32(_size.Width);
			var height = Convert.ToUInt32(_size.Height);
			var pixelData = new Span<byte>(pixels.RawPixels, Convert.ToInt32(pixels.Size));
			_device?.UpdateTexture(_texture, pixelData, 0, 0, 0, width, height, 1, 0, 0);
		}

		private void CreateTexture() {
			var factory = _device.ResourceFactory;
			var pixels = _view.Surface?.Bitmap ?? null;
			if (pixels != null) throw new Exception("Could not acquire web view viewport bitmap.");

			_texture?.Dispose();
			_texture = factory.CreateTexture(TextureDescription.Texture2D(
				Convert.ToUInt32(_size.Width), Convert.ToUInt32(_size.Height),
				1, 1, ConvertPixelFormat(pixels.Format), TextureUsage.Sampled));

			TextureRecreated?.Invoke(this, new EventArgs());
		}

		private static PixelFormat ConvertPixelFormat(ULBitmapFormat format) {
			return format switch {
				ULBitmapFormat.A8_UNORM => PixelFormat.R8_G8_B8_A8_UNorm,
				ULBitmapFormat.BGRA8_UNORM_SRGB => PixelFormat.B8_G8_R8_A8_UNorm_SRgb,
				_ => throw new FormatException("Unsupported web view pixel format!"),
			};
		}
	}

	internal sealed class FontLoader : IFontLoader {
		private static readonly IList<ULFontFile> _fonts = [];

		public string GetFallbackFont() {
			return "Times New Roman";
		}

		public string GetFallbackFontForCharacters(string text, int weight, bool italic) {
			return GetFallbackFont();
		}

		public ULFontFile Load(string fontFamliy, int weight, bool italic) {
			var family = SystemFonts.Families.FirstOrDefault(
				family => family.Name.Contains(fontFamliy, StringComparison.InvariantCultureIgnoreCase)
			);
			var font = family.CreateFont(32, italic ? FontStyle.Italic : FontStyle.Regular);
			Debug.Assert(font.TryGetPath(out string path));

			var ulFont = ULFontFile.CreateFromFile(path.AsSpan());
			_fonts.Add(ulFont);
			return ulFont;
		}

		public void Dispose() {
			foreach (var font in _fonts) font.Dispose();
			_fonts.Clear();
		}
	}
}
