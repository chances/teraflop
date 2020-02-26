using System.Linq;
using System.Runtime.CompilerServices;
using OpenTK.Graphics.ES30;

namespace Teraflop.Buffers.Uniforms
{
    public class UniformBuffer<T> : Buffer where T : struct
    {
        private T[] _uniformData;

        public UniformBuffer()
        {
            UniformData = new T();
        }

        public UniformBuffer(T uniformData)
        {
            UniformData = uniformData;
        }

        public T UniformData
        {
            private get => _uniformData.FirstOrDefault();
            set
            {
                _uniformData = new T[] { value };
                if (Initialized)
                {
                    Update();
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            Update();
        }

        public void Bind(int shaderHandle, string name)
        {
            var location = GL.GetUniformLocation(shaderHandle, name);

            if (UniformData is System.Numerics.Matrix4x4 matrix)
                GL.UniformMatrix4(location, 16, false, matrix.ToArray());
            if (UniformData is OpenTK.Graphics.Color4 color)
                GL.Uniform4(location, color);
        }

        private void Update()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, DeviceBuffer.Value);
            GL.BufferData(BufferTarget.UniformBuffer, Unsafe.SizeOf<T>(), _uniformData, BufferUsageHint.StreamDraw);
        }
    }

    public static class MatrixExtensions
    {
        public static float[] ToArray(this System.Numerics.Matrix4x4 matrix)
        {
            return new float[] {
                matrix.M11, matrix.M12, matrix.M13, matrix.M14,
                matrix.M21, matrix.M22, matrix.M23, matrix.M24,
                matrix.M31, matrix.M32, matrix.M33, matrix.M34,
                matrix.M41, matrix.M42, matrix.M43, matrix.M44
            };
        }
    }
}
