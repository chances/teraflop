using System.Numerics;
using Teraflop.Buffers.Uniforms;

namespace Teraflop.Components.Receivers
{
    public interface ICameraViewProjection : IDependencies
    {
        UniformBuffer<Matrix4x4> CameraViewProjection { set; }
    }
}
