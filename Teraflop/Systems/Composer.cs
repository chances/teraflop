using System.Linq;
using Teraflop.ECS;

namespace Teraflop.Systems {
	public class Composer : ECS.System<Composition> {
		public Composer(World world) : base(world) {
		}

		public override void Operate() {
			var uninitializedCompositions = OperableEntities
				.Where(entity => entity.GetComponent<Composition>().Initialized == false);

			foreach (var entity in uninitializedCompositions) {
				var composition = entity.GetComponent<Composition>();
				var providers = entity.GetComponents<ComposableResource>()
					.Where(provider => composition.IsComposable(provider.GetType()))
					.ToList();
				providers.ForEach(composition.Compose);
			}
		}
	}
}
