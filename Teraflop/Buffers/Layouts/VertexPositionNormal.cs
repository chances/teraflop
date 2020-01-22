using System.Linq;
using System.Numerics;

namespace Teraflop.Buffers.Layouts
{
    public struct VertexPositionNormal : IVertexBufferDescription
    {
        public Vector3 Position;
        public Vector3 Normal;

        public VertexPositionNormal(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
        }

        private static readonly VertexLayoutDescription _layoutDescription = new VertexLayoutDescription(
            new VertexElementDescription(nameof(Position), VertexElementSemantic.Position, VertexElementFormat.Float3),
            new VertexElementDescription(nameof(Normal), VertexElementSemantic.Normal, VertexElementFormat.Float3,
                VertexLayoutHelpers.GetSizeInBytes(VertexElementFormat.Float3)));

        public VertexLayoutDescription LayoutDescription => _layoutDescription;

        public uint SizeInBytes => _layoutDescription.Elements.Aggregate(
            (uint) 0, (sum, element) => sum += element.SizeInBytes);
    }
}
