using System.Numerics;
using Teraflop.Buffers.Uniforms;

namespace Teraflop.Components.Receivers
{
    public interface IModelTransformation
    {
        Matrix4x4 ModelTransformation { set; }
    }
}
