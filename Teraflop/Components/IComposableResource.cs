using System.Collections.Generic;
using Veldrid;

namespace Teraflop.Components {
	public interface IComposableResource {
		IEnumerable<ResourceLayoutElementDescription> ResourceLayout { get; }
		IEnumerable<BindableResource> ResourceSet { get; }
	}
}
