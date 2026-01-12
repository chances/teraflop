using Assimp.Unmanaged;
using System;
using System.Collections.Generic;
using System.Drawing;
using Teraflop.Components.Receivers;
using Teraflop.ECS;
using Teraflop.Primitives;
using Veldrid;

namespace Teraflop.Components.UI {
	public class Surface : Resource, IReady, IUpdatable, IBindableResource {
		private GraphicsDevice _device;
		private readonly WebView _view;
		private TextureView _textureView;
		private Sampler _sampler;

		public Surface(WebView view) : base("UI Surface") {
			_view = view;
			_view.TextureRecreated += (_, __) => CreateTextureView();

			Resources.Initializing += (_, e) => {
				_device = e.GraphicsDevice;

				_sampler = _device.LinearSampler;

				ResourceLayout = e.ResourceFactory.CreateResourceLayout(
					new ResourceLayoutDescription(
						new ResourceLayoutElementDescription(
							"SurfaceTexture", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
						new ResourceLayoutElementDescription(
							"SurfaceSampler", ResourceKind.Sampler, ShaderStages.Fragment)));

				CreateTextureView();
			};
			Resources.Disposed += (_, __) => {
				_textureView.Dispose();
				_view.Dispose();
				ResourceLayout.Dispose();
				ResourceSet.Dispose();
			};
		}

		public static readonly MeshData Mesh = MeshBuilder.TexturedUnitQuad("UI Surface Mesh");

		public bool IsReady => true;

		public ResourceLayout ResourceLayout { get; private set; }

		public ResourceSet ResourceSet { get; private set; }

		public void Update(GameTime gameTime) {
			_view.Update(gameTime);
		}

		private void CreateTextureView() {
			var factory = _device.ResourceFactory;

			_textureView?.Dispose();
			_textureView = factory.CreateTextureView(_view.Texture);

			ResourceSet?.Dispose();
			ResourceSet = factory.CreateResourceSet(new ResourceSetDescription(
				ResourceLayout, _textureView, _sampler
			));
		}
	}
}
