using System.IO;
using System.Threading.Tasks;

namespace Teraflop.Assets
{
    public interface IAssetImporter<T>
    {
        Task<T> Import(Stream assetData);
    }
}
