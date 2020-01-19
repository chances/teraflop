using OpenTK.Graphics;

namespace Teraflop.Buffers.Uniforms
{
    public class UniformColor : IUniformBufferDescription<Color4>
    {
        public UniformBuffer<Color4> Buffer { get; private set; }

        public UniformColor()
        {
            Buffer = new UniformBuffer<Color4>();
        }

        public UniformColor(Color4 color)
        {
            Buffer = new UniformBuffer<Color4>(color);
        }

        public ResourceLayoutElementDescription LayoutDescription => UniformColor.ResourceLayout;

        public static ResourceLayoutElementDescription ResourceLayout =>
            new ResourceLayoutElementDescription("Color", ResourceKind.UniformBuffer, ShaderStages.Fragment);
    }
}
