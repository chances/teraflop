using System.Numerics;
using OpenTK.Graphics;

namespace Teraflop.Buffers.Layouts
{
    public struct VertexPositionColorTexture : IVertexBufferDescription
    {
        public Vector3 Position;
        public Vector4 Color;
        public Vector2 TexCoordinates;

        public VertexPositionColorTexture(Vector3 position, Color4 color, Vector2 texCoordinates)
        {
            Position = position;
            Color = new Vector4(color.A, color.R, color.G, color.B);
            TexCoordinates = texCoordinates;
        }

        private static readonly VertexLayoutDescription _layoutDescription = new VertexLayoutDescription(
            new VertexElementDescription(nameof(Position),
                VertexElementSemantic.Position, VertexElementFormat.Float3),
            new VertexElementDescription(nameof(Color),
                VertexElementSemantic.Color, VertexElementFormat.Float4),
            new VertexElementDescription(nameof(TexCoordinates),
                VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2)
        );

        public VertexLayoutDescription LayoutDescription => _layoutDescription;

        // float is 4 bytes(?), 4*9=36
        public uint SizeInBytes => 36;
    }
}
