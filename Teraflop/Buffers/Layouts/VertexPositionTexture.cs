using System.Numerics;
using Veldrid;

namespace Teraflop.Buffers.Layouts
{
    public struct VertexPositionTexture : IVertexBufferDescription
    {
        public Vector3 Position;
        public Vector2 TexCoordinates;

        public VertexPositionTexture(Vector3 position, Vector2 texCoordinates)
        {
            Position = position;
            TexCoordinates = texCoordinates;
        }

        private static readonly VertexLayoutDescription _layoutDescription = new VertexLayoutDescription(
            new VertexElementDescription(nameof(Position), VertexElementSemantic.Position, VertexElementFormat.Float3),
            new VertexElementDescription(nameof(TexCoordinates), VertexElementSemantic.TextureCoordinate,
                VertexElementFormat.Float2));

        public VertexLayoutDescription LayoutDescription => _layoutDescription;

        // float is 4 bytes(?), 4*5=20
        public uint SizeInBytes => 20;
    }
}
