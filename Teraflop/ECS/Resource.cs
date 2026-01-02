using System.Collections.Generic;
using Teraflop.Components;
using Veldrid;

namespace Teraflop.ECS {
	public abstract class Resource : Component, IResource {
		protected Resource(string name) : base(name) {
		}

		protected Resources Resources { get; } = new Resources();

		public bool Initialized => Resources.Initialized;

		public void Initialize(ResourceFactory factory, GraphicsDevice device) =>
			Resources.Initialize(factory, device);

		public void Dispose() {
			Resources.Dispose();
		}
	}

	public abstract class ComposableResource : Resource, IComposableResource {
		protected ComposableResource(string name) : base(name) {
		}

		public abstract IEnumerable<ResourceLayoutElementDescription> ResourceLayout { get; }
		public abstract IEnumerable<BindableResource> ResourceSet { get; }
	}
}
