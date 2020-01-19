using System;

namespace Teraflop
{
    public enum ResourceKind
    {
        UniformBuffer,
        Texture2D
    }

    [Flags]
    public enum ShaderStages : byte
    {
        Compute = 0x1,
        Vertex = 0xA,
        Fragment = 0xB
    }

    public struct ResourceLayoutElementDescription
    {
        /// <summary>
        /// The name of the element.
        /// </summary>
        public string Name;
        //
        // Summary:

        /// <summary>
        /// The kind of resource.
        /// </summary>
        public ResourceKind Kind;
        //
        // Summary:

        /// <summary>
        /// The Veldrid.ShaderStages in which this element is used.
        /// </summary>
        public ShaderStages Stages;

        /// <summary>
        /// Constructs a new ResourceLayoutElementDescription.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        /// <param name="kind">The kind of resource.</param>
        /// <param name="stages">The Veldrid.ShaderStages in which this element is used.</param>
        public ResourceLayoutElementDescription(string name, ResourceKind kind, ShaderStages stages)
        {
            Name = name;
            Kind = kind;
            Stages = stages;
        }
    }
}
