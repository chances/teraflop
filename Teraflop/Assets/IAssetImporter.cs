using System.IO;

namespace Teraflop.Assets
{
    public interface IAssetImporter<out T>
    {
        T Import(Stream assetData);
    }
}
