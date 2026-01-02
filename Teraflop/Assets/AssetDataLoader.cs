using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using LiteGuard;

namespace Teraflop.Assets {
	internal class AssetDataLoader : IAssetSource {
		private readonly ObservableCollection<IAssetSource> _assetSources =
			new ObservableCollection<IAssetSource>();
		private readonly Dictionary<string, IAssetSource> _assetSourceByFile =
			new Dictionary<string, IAssetSource>();

		public AssetDataLoader() {
			_assetSources.CollectionChanged += (_, e) => {
				var oldItems = e.OldItems?.Cast<IAssetSource>() ?? new List<IAssetSource>();
				var newItems = e.NewItems?.Cast<IAssetSource>() ?? new List<IAssetSource>();
				oldItems.SelectMany(source => source.AssetFilenames).ToList()
					.ForEach(file => _assetSourceByFile.Remove(file));
				newItems.ToList().ForEach(source => source.AssetFilenames.ToList()
					.ForEach(file => _assetSourceByFile[file] = source));
			};
		}

		public IList<IAssetSource> AssetSources => _assetSources;
		public IEnumerable<string> AssetFilenames => _assetSourceByFile.Keys;

		/// <summary>
		/// Load an asset from the asset library given a <paramref name="type"/> and a <paramref name="filePath"/>.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="filePath"></param>
		/// <returns><see cref="Stream"/> of loaded asset's data</returns>
		/// <exception cref="ArgumentNullException"><paramref name="filePath"/> is null</exception>
		/// <exception cref="FileNotFoundException">Given <paramref name="filePath"/> doesn't exist in asset library</exception>
		public Stream Load(AssetType type, [NotNull] string filePath) {
			Guard.AgainstNullArgument(nameof(filePath), filePath);
			if (!this.Exists(filePath)) {
				throw new FileNotFoundException($"{type} '{filePath}' does not exist in the asset library");
			}

			return _assetSourceByFile[filePath].Load(type, filePath);
		}
	}
}
