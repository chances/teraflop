// Borrowed from https://github.com/mellinoe/veldrid

namespace Teraflop.Buffers
{
    /// <summary>
    /// Describes a single element of a vertex.
    /// </summary>
    public struct VertexElementDescription
    {
        /// <summary>
        /// The name of the element.
        /// </summary>
        public string Name;
        /// <summary>
        /// The semantic type of the element.
        /// </summary>
        public VertexElementSemantic Semantic;
        /// <summary>
        /// The format of the element.
        /// </summary>
        public VertexElementFormat Format;
        /// <summary>
        /// The offset in bytes from the beginning of the vertex.
        /// </summary>
        public uint Offset;

        /// <summary>
        /// The size in bytes of the element.
        /// </summary>
        public uint SizeInBytes => VertexLayoutHelpers.GetSizeInBytes(Format);

        /// <summary>
        /// Constructs a new VertexElementDescription describing a per-vertex element.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        /// <param name="semantic">The semantic type of the element.</param>
        /// <param name="format">The format of the element.</param>
        public VertexElementDescription(string name, VertexElementSemantic semantic, VertexElementFormat format)
            : this(name, format, semantic)
        {
        }

        /// <summary>
        /// Constructs a new VertexElementDescription.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        /// <param name="semantic">The semantic type of the element.</param>
        /// <param name="format">The format of the element.</param>
        public VertexElementDescription(
            string name,
            VertexElementFormat format,
            VertexElementSemantic semantic)
        {
            Name = name;
            Format = format;
            Semantic = semantic;
            Offset = 0;
        }

        /// <summary>
        /// Constructs a new VertexElementDescription.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        /// <param name="semantic">The semantic type of the element.</param>
        /// <param name="format">The format of the element.</param>
        /// <param name="offset">The offset in bytes from the beginning of the vertex.</param>
        public VertexElementDescription(
            string name,
            VertexElementSemantic semantic,
            VertexElementFormat format,
            uint offset)
        {
            Name = name;
            Format = format;
            Semantic = semantic;
            Offset = offset;
        }
    }
}
