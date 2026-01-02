using System.Linq;
using System.Threading.Tasks;
using Teraflop.Assets;
using Teraflop.ECS;

namespace Teraflop.Systems {
	internal class ComponentAssetLoader : System<IAssetSink> {
		private readonly AssetDataLoader _assetDataLoader;

		public ComponentAssetLoader(World world, AssetDataLoader assetDataLoader) : base(world) {
			_assetDataLoader = assetDataLoader;
		}

		public override void Operate() {
			var loadingTask = Task.WhenAll(OperableComponents
				.Select(assetSink => assetSink.LoadAssets(_assetDataLoader))
			);
			loadingTask.GetAwaiter().OnCompleted(() => {
				var exception = loadingTask.Exception?.Flatten();
				if (exception != null) {
					throw exception.InnerExceptions.First();
				}
				// TODO: Some kinda notifier for "Loading..." screens
			});
		}
	}
}
