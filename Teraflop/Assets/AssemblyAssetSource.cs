using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetBrains.Annotations;

namespace Teraflop.Assets
{
    public class AssemblyAssetSource : IAssetSource
    {
        private readonly Assembly _gameAssembly;

        /// <summary>
        /// Instantiate a new <see cref="IAssetSource"/> that loads assets from a <see cref="Assembly"/>.
        /// </summary>
        /// <param name="assembly">Assembly from which to load assets. Defaults to <see cref="Assembly.GetCallingAssembly"/>.</param>
        public AssemblyAssetSource(Assembly assembly = null)
        {
            _gameAssembly = assembly ?? Assembly.GetCallingAssembly();
        }

        public IEnumerable<string> AssetFilenames => _gameAssembly.GetManifestResourceNames();

        public Stream Load(AssetType type, [NotNull] string filePath)
        {
            return _gameAssembly.GetManifestResourceStream(filePath);
        }
    }
}
