using System.Threading.Tasks;

namespace Teraflop.Assets {
	public interface IAssetSink {
		Task LoadAssets(IAssetSource assetSource);
	}
}
