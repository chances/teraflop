// Borrowed from https://github.com/mellinoe/veldrid

using System;

namespace Teraflop.Buffers
{
    /// <summary>
    /// Describes the layout of vertex data in a single Device Buffer used as a vertex buffer.
    /// </summary>
    public struct VertexLayoutDescription
    {
        /// <summary>
        /// The number of bytes in between successive elements in the Device Buffer.
        /// </summary>
        public uint Stride;
        /// <summary>
        /// An array of <see cref="VertexElementDescription"/> objects, each describing a single element of vertex data.
        /// </summary>
        public VertexElementDescription[] Elements;
        /// <summary>
        /// A value controlling how often data for instances is advanced for this layout. For per-vertex elements, this value
        /// should be 0.
        /// For example, an InstanceStepRate of 3 indicates that 3 instances will be drawn with the same value for this layout. The
        /// next 3 instances will be drawn with the next value, and so on.
        /// </summary>
        public uint InstanceStepRate;

        /// <summary>
        /// Constructs a new VertexLayoutDescription.
        /// </summary>
        /// <param name="stride">The number of bytes in between successive elements in the Device Buffer.</param>
        /// <param name="elements">An array of <see cref="VertexElementDescription"/> objects, each describing a single element
        /// of vertex data.</param>
        public VertexLayoutDescription(uint stride, params VertexElementDescription[] elements)
        {
            Stride = stride;
            Elements = elements;
            InstanceStepRate = 0;
        }

        /// <summary>
        /// Constructs a new VertexLayoutDescription.
        /// </summary>
        /// <param name="stride">The number of bytes in between successive elements in the Device Buffer.</param>
        /// <param name="elements">An array of <see cref="VertexElementDescription"/> objects, each describing a single element
        /// of vertex data.</param>
        /// <param name="instanceStepRate">A value controlling how often data for instances is advanced for this element. For
        /// per-vertex elements, this value should be 0.
        /// For example, an InstanceStepRate of 3 indicates that 3 instances will be drawn with the same value for this element.
        /// The next 3 instances will be drawn with the next value for this element, and so on.</param>
        public VertexLayoutDescription(uint stride, uint instanceStepRate, params VertexElementDescription[] elements)
        {
            Stride = stride;
            Elements = elements;
            InstanceStepRate = instanceStepRate;
        }

        /// <summary>
        /// Constructs a new VertexLayoutDescription. The stride is assumed to be the sum of the size of all elements.
        /// </summary>
        /// <param name="elements">An array of <see cref="VertexElementDescription"/> objects, each describing a single element
        /// of vertex data.</param>
        public VertexLayoutDescription(params VertexElementDescription[] elements)
        {
            Elements = elements;
            uint computedStride = 0;
            for (int i = 0; i < elements.Length; i++)
            {
                uint elementSize = VertexLayoutHelpers.GetSizeInBytes(elements[i].Format);
                if (elements[i].Offset != 0)
                {
                    computedStride = elements[i].Offset + elementSize;
                }
                else
                {
                    computedStride += elementSize;
                }
            }

            Stride = computedStride;
            InstanceStepRate = 0;
        }
    }

    internal class VertexLayoutHelpers
    {
        public static uint GetSizeInBytes(VertexElementFormat format)
        {
            switch (format)
            {
                case VertexElementFormat.Byte2_Norm:
                case VertexElementFormat.Byte2:
                case VertexElementFormat.SByte2_Norm:
                case VertexElementFormat.SByte2:
                    return 2;
                case VertexElementFormat.Float1:
                case VertexElementFormat.UInt1:
                case VertexElementFormat.Int1:
                case VertexElementFormat.Byte4_Norm:
                case VertexElementFormat.Byte4:
                case VertexElementFormat.SByte4_Norm:
                case VertexElementFormat.SByte4:
                case VertexElementFormat.UShort2_Norm:
                case VertexElementFormat.UShort2:
                case VertexElementFormat.Short2_Norm:
                case VertexElementFormat.Short2:
                    return 4;
                case VertexElementFormat.Float2:
                case VertexElementFormat.UInt2:
                case VertexElementFormat.Int2:
                case VertexElementFormat.UShort4_Norm:
                case VertexElementFormat.UShort4:
                case VertexElementFormat.Short4_Norm:
                case VertexElementFormat.Short4:
                    return 8;
                case VertexElementFormat.Float3:
                case VertexElementFormat.UInt3:
                case VertexElementFormat.Int3:
                    return 12;
                case VertexElementFormat.Float4:
                case VertexElementFormat.UInt4:
                case VertexElementFormat.Int4:
                    return 16;
                default:
                    throw new ArgumentException("Unknown vertex element format", nameof(format));
            }
        }

        public static int GetElementCount(VertexElementFormat format)
        {
            switch (format)
            {
                case VertexElementFormat.Float1:
                case VertexElementFormat.UInt1:
                case VertexElementFormat.Int1:
                    return 1;
                case VertexElementFormat.Float2:
                case VertexElementFormat.Byte2_Norm:
                case VertexElementFormat.Byte2:
                case VertexElementFormat.SByte2_Norm:
                case VertexElementFormat.SByte2:
                case VertexElementFormat.UShort2_Norm:
                case VertexElementFormat.UShort2:
                case VertexElementFormat.Short2_Norm:
                case VertexElementFormat.Short2:
                case VertexElementFormat.UInt2:
                case VertexElementFormat.Int2:
                    return 2;
                case VertexElementFormat.Float3:
                case VertexElementFormat.UInt3:
                case VertexElementFormat.Int3:
                    return 3;
                case VertexElementFormat.Float4:
                case VertexElementFormat.Byte4_Norm:
                case VertexElementFormat.Byte4:
                case VertexElementFormat.SByte4_Norm:
                case VertexElementFormat.SByte4:
                case VertexElementFormat.UShort4_Norm:
                case VertexElementFormat.UShort4:
                case VertexElementFormat.Short4_Norm:
                case VertexElementFormat.Short4:
                case VertexElementFormat.UInt4:
                case VertexElementFormat.Int4:
                    return 4;
                default:
                    throw new ArgumentException("Unknown vertex element format", nameof(format));
            }
        }
    }
}
