using System.Numerics;

namespace Teraflop.Buffers.Uniforms
{
    public abstract class UniformMatrix
    {
        public UniformBuffer<Matrix4x4> Buffer { get; private set; }

        public UniformMatrix()
        {
            Buffer = new UniformBuffer<Matrix4x4>();
        }

        public UniformMatrix(Matrix4x4 value)
        {
            Buffer = new UniformBuffer<Matrix4x4>(value);
        }
    }
}
