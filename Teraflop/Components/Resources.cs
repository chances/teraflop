using System;
using Veldrid;

namespace Teraflop.Components {
	public class Resources : IResource {
		public event EventHandler<InitializingEventArgs> Initializing;
		public event EventHandler Disposed;
		public bool Initialized { get; private set; } = false;

		public void Initialize(ResourceFactory factory, GraphicsDevice device) {
			var handler = Initializing;
			if (handler != null) {
				handler(this, new InitializingEventArgs(factory, device));
				Initialized = true;
			}
		}

		public void Dispose() {
			var handler = Disposed;
			if (handler != null) {
				handler(this, new EventArgs());
				Initialized = false;
			}
		}
	}

	public class InitializingEventArgs(ResourceFactory factory, GraphicsDevice device) : EventArgs {
		public ResourceFactory ResourceFactory { get; } = factory;
		public GraphicsDevice GraphicsDevice { get; } = device;
	}
}
