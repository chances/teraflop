using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Veldrid;

namespace Teraflop.Buffers.Uniforms {
	public class UniformBuffer<T> : Buffer where T : unmanaged {
		private GraphicsDevice _device;
		[NotNull]
		private T _uniformData;

		public UniformBuffer() {
			UniformData = new T();
		}

		public UniformBuffer(T uniformData) {
			UniformData = uniformData;
		}

		public T UniformData {
			get => _uniformData;
			set {
				_uniformData = value;
				if (Initialized) {
					Update();
				}
			}
		}

		public override void Initialize(ResourceFactory factory, GraphicsDevice device) {
			_buffer = factory.CreateBuffer(
				new BufferDescription((uint)Unsafe.SizeOf<T>(), BufferUsage.UniformBuffer | BufferUsage.Dynamic));
			_device = device;
			Update();
		}

		private void Update() {
			_device.UpdateBuffer(_buffer, 0, _uniformData);
		}
	}
}
