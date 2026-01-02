using System;
using Veldrid;

namespace Teraflop.Components {
	public class Resources : IResource {
		public event EventHandler<InitializeEventArgs> OnInitialize;
		public event EventHandler OnDispose;
		public bool Initialized { get; private set; } = false;

		public void Initialize(ResourceFactory factory, GraphicsDevice device) {
			var handler = OnInitialize;
			if (handler != null) {
				handler(this, new InitializeEventArgs(factory, device));
				Initialized = true;
			}
		}

		public void Dispose() {
			var handler = OnDispose;
			if (handler != null) {
				handler(this, new EventArgs());
				Initialized = false;
			}
		}
	}

	public class InitializeEventArgs : EventArgs {
		public InitializeEventArgs(ResourceFactory factory, GraphicsDevice device) {
			ResourceFactory = factory;
			GraphicsDevice = device;
		}
		public ResourceFactory ResourceFactory { get; }
		public GraphicsDevice GraphicsDevice { get; }
	}
}
