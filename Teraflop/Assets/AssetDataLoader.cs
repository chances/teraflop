using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using LiteGuard;

namespace Teraflop.Assets
{
    internal class AssetDataLoader : IAssetSource
    {
        private readonly Dictionary<string, IAssetSource> _assetFiles =
            new Dictionary<string, IAssetSource>();

        public AssetDataLoader()
        {
            AssetSources.CollectionChanged += (_, e) => {
                var oldItems = e.OldItems?.Cast<IAssetSource>() ?? new List<IAssetSource>();
                var newItems = e.NewItems?.Cast<IAssetSource>() ?? new List<IAssetSource>();
                oldItems.SelectMany(source => source.AssetFilenames).ToList()
                    .ForEach(file => _assetFiles.Remove(file));
                newItems.ToList().ForEach(source => source.AssetFilenames.ToList()
                    .ForEach(file => _assetFiles[file] = source));
            };
        }

        public ObservableCollection<IAssetSource> AssetSources { get; } =
            new ObservableCollection<IAssetSource>();

        public IEnumerable<string> AssetFilenames => _assetFiles.Keys;

        /// <summary>
        /// Load an asset from the asset library given a <paramref name="type"/> and a <paramref name="filePath"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="filePath"></param>
        /// <returns><see cref="Stream"/> of loaded asset's data</returns>
        /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is null</exception>
        /// <exception cref="FileNotFoundException">Given <paramref name="filePath"/> doesn't exist in asset library</exception>
        public Stream Load(AssetType type, [NotNull] string filePath)
        {
            Guard.AgainstNullArgument(nameof(filePath), filePath);
            if (!Exists(type, filePath))
            {
                throw new FileNotFoundException($"{type} '{filePath}' does not exist in the asset library");
            }

            return _assetFiles[filePath].Load(type, filePath);
        }

        public bool Exists(AssetType type, [NotNull] string filePath)
        {
            Guard.AgainstNullArgument(nameof(filePath), filePath);
            return _assetFiles.ContainsKey(filePath);
        }
    }
}
