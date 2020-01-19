using System;
using JetBrains.Annotations;
using LiteGuard;
using OpenTK.Graphics.ES20;

namespace Teraflop.Buffers
{
    public class IndexBuffer : Buffer
    {
        private readonly ushort[] _indices;

        public IndexBuffer([NotNull] ushort[] indices)
        {
            Guard.AgainstNullArgument(nameof(indices), indices);
            if (indices.Length == 0)
            {
                throw new ArgumentException("Given index data must not be empty.", nameof(indices));
            }
            _indices = indices;
        }

        public int Count => _indices.Length;

        public override void Initialize()
        {
            base.Initialize();

            GL.BindBuffer(BufferTarget.ArrayBuffer, DeviceBuffer.Value);
            var size = (int) (_indices.Length * sizeof(ushort));
            GL.BufferData(BufferTarget.ArrayBuffer, size, _indices, BufferUsageHint.StaticDraw);
        }
    }
}
