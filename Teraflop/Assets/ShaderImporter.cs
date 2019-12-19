using System.IO;

namespace Teraflop.Assets
{
    public class ShaderImporter : IAssetImporter<byte[]>
    {
        public static ShaderImporter Instance = new ShaderImporter();

        public byte[] Import(Stream assetData)
        {
            using (var stream = new MemoryStream())
            {
                // TODO: Use CopyToAsync when we need "Loading..." screen
                assetData.CopyTo(stream);
                return stream.ToArray();
            }
        }
    }
}
