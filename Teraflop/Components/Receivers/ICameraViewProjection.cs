using System.Numerics;
using Teraflop.Buffers.Uniforms;
using Teraflop.Input;

namespace Teraflop.Components.Receivers
{
    public interface ICameraViewProjection
    {
        UniformBuffer<Matrix4x4> CameraViewProjection { set; }
    }
}
