using System.Collections.Generic;
using Teraflop.Buffers.Uniforms;
using Teraflop.ECS;
using Veldrid;

namespace Teraflop.Components {
	public class Color : ComposableResource {
		private UniformColor _color;

		public Color(RgbaFloat? color = null) : base(nameof(Color)) {
			_color = color.HasValue ? new UniformColor(color.Value) : new UniformColor();

			Resources.Initializing += (_, e) => {
				_color.Buffer.Initialize(e.ResourceFactory, e.GraphicsDevice);
			};
			Resources.Disposed += (_, __) => _color.Buffer.Dispose();
		}

		public RgbaFloat Value {
			get => _color.Buffer.UniformData;
			set => _color.Buffer.UniformData = value;
		}

		#region Veldrid
		public override IEnumerable<ResourceLayoutElementDescription> ResourceLayout =>
			new ResourceLayoutElementDescription[] { _color.LayoutDescription };
		public override IEnumerable<BindableResource> ResourceSet => new BindableResource[] {
		  _color.Buffer.DeviceBuffer
		};
		#endregion
	}
}
