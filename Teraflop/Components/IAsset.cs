using Teraflop.Assets;

namespace Teraflop.Components
{
    public interface IAsset : IResource
    {
        void LoadAssets(AssetDataLoader assetDataLoader);
    }
}
