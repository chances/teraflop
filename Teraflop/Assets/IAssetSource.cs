using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace Teraflop.Assets {
	public interface IAssetSource {
		IEnumerable<string> AssetFilenames { get; }
		Stream Load(AssetType type, [NotNull] string filePath);
	}

	public static class AssetSourceExtensions {
		public static bool Exists(this IAssetSource assetSource, string filePath) {
			return assetSource.AssetFilenames.Contains(filePath);
		}

		public static string GetAbsolutePath(this IAssetSource assetSource, string fileName) {
			return assetSource.AssetFilenames.First(filePath => filePath.EndsWith(fileName));
		}
	}
}
