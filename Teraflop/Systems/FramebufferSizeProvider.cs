using System;
using System.Drawing;
using Teraflop.Components.Receivers;
using Teraflop.ECS;
using Veldrid;

namespace Teraflop.Systems
{
    public class FramebufferSizeProvider : System<IFramebufferSize>
    {
        private Size _size;
        private int _oldSize;

        public FramebufferSizeProvider(World world, uint width, uint height) : base(world)
        {
            Update((int) width, (int) height);
        }

        public void Update(int width, int height)
        {
            _size = new Size(width, height);
        }

        public override void Operate()
        {
            if (IsDirty)
            {
                foreach (var componentToUpdate in OperableComponents)
                {
                    componentToUpdate.FramebufferSize = _size;
                }
            }

            _oldSize = Size;
        }

        private int Size => _size.Width * _size.Height;
        private bool IsDirty => _oldSize > 0 && Size != _oldSize;
    }
}
