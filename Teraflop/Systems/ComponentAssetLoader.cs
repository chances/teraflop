using System.Linq;
using Teraflop.Assets;
using Teraflop.Components;
using Teraflop.ECS;

namespace Teraflop.Systems
{
    public class ComponentAssetLoader : System<IAsset>
    {
        private readonly AssetDataLoader _assetDataLoader;

        public ComponentAssetLoader(World world, AssetDataLoader assetDataLoader) : base(world)
        {
            _assetDataLoader = assetDataLoader;
        }

        public override void Operate()
        {
            // Load assets for resources that are uninitalized
            foreach (var asset in OperableComponents
                .Where(component => (component is IResource resource && !resource.Initialized)))
            {
                asset.LoadAssets(_assetDataLoader);
            }
        }
    }
}
