using System.Linq;
using System.Runtime.CompilerServices;
using OpenTK.Graphics.ES20;

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

        private void Update()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, DeviceBuffer.Value);
            GL.BufferData(BufferTarget.ArrayBuffer, Unsafe.SizeOf<T>(), _uniformData, BufferUsageHint.StreamDraw);
        }
    }
}
