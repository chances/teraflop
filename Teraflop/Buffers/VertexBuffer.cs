using System;
using System.Linq;
using JetBrains.Annotations;
using LiteGuard;
using OpenTK.Graphics.ES20;

namespace Teraflop.Buffers
{
    public abstract class VertexBuffer : Buffer
    {
        public abstract VertexLayoutDescription LayoutDescription { get; }

        public abstract int VertexBufferHandle { get; }

        public IndexBuffer Indices { get; protected set; }
    }

    public class VertexBuffer<T> : VertexBuffer where T : struct, IVertexBufferDescription
    {
        private readonly T[] _vertices;

        public VertexBuffer([NotNull] IVertexBufferDescription[] vertices, [NotNull] ushort[] indices)
        {
            Guard.AgainstNullArgument(nameof(vertices), vertices);
            Guard.AgainstNullArgument(nameof(indices), indices);
            if (vertices.Length == 0)
            {
                throw new ArgumentException("Given vertices must not be empty.", nameof(vertices));
            }

            _vertices = vertices.Cast<T>().ToArray();
            Indices = new IndexBuffer(indices);
        }

        public override VertexLayoutDescription LayoutDescription => _vertices[0].LayoutDescription;

        public override int VertexBufferHandle => DeviceBuffer.Value;

        public override void Initialize()
        {
            base.Initialize();

            GL.BindBuffer(BufferTarget.ArrayBuffer, DeviceBuffer.Value);
            var size = (int) (_vertices.Length * _vertices[0].SizeInBytes);
            GL.BufferData(BufferTarget.ArrayBuffer, size, _vertices, BufferUsageHint.StaticDraw);

            Indices.Initialize();
        }

        public new void Dispose()
        {
            base.Dispose();
            Indices.Dispose();
        }
    }
}
