using System;
using System.Drawing;
using System.Numerics;
using Teraflop.Buffers.Uniforms;
using Teraflop.Components.Receivers;
using Teraflop.ECS;

namespace Teraflop.Systems
{
    public class ViewProjectionProvider : System<ICameraViewProjection>
    {
        private UniformBuffer<Matrix4x4> _cameraViewProjection;

        public ViewProjectionProvider(World world, UniformBuffer<Matrix4x4> cameraViewProjection) : base(world)
        {
            _cameraViewProjection = cameraViewProjection;
        }

        public override void Operate()
        {
            foreach (var componentToUpdate in OperableComponents)
            {
                componentToUpdate.CameraViewProjection = _cameraViewProjection;
            }
        }
    }
}
