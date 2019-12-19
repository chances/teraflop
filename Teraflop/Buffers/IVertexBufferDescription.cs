using Veldrid;

namespace Teraflop.Buffers
{
    public interface IVertexBufferDescription
    {
        VertexLayoutDescription LayoutDescription { get; }
        uint SizeInBytes { get; }
    }
}
