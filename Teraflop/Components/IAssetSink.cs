using System.Threading.Tasks;
using Teraflop.Assets;

namespace Teraflop.Components
{
    public interface IAssetSink
    {
        Task LoadAssets(IAssetSource assetSource);
    }
}
