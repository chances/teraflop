using Teraflop.Components;
using Teraflop.ECS;

namespace Teraflop.Systems {
	public class ResourceDisposal : System<IResource> {
		public ResourceDisposal(World world) : base(world) {
		}

		public override void Operate() {
			foreach (var resource in OperableComponents) {
				resource.Dispose();
			}
		}
	}
}
