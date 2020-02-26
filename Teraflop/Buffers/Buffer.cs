using System;
using OpenTK.Graphics.ES30;

namespace Teraflop.Buffers
{
    public abstract class Buffer : IBufferResource, IDisposable
    {
        public string Name { get; set; }

        public int? DeviceBuffer { get; private set; } = null;

        public bool Initialized => DeviceBuffer.HasValue;

        public void Dispose()
        {
            if (DeviceBuffer.HasValue)
            {
                GL.DeleteBuffer(DeviceBuffer.Value);
            }
            DeviceBuffer = null;
        }

        public virtual void Initialize()
        {
            DeviceBuffer = GL.GenBuffer();
        }
    }
}
