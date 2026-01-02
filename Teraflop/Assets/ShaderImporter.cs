using System.IO;
using System.Threading.Tasks;

namespace Teraflop.Assets {
	public class ShaderImporter : IAssetImporter<byte[]> {
		public static ShaderImporter Instance = new ShaderImporter();

		public async Task<byte[]> Import(Stream assetData) {
			using var stream = new MemoryStream();
			await assetData.CopyToAsync(stream);
			return stream.ToArray();
		}
	}
}
