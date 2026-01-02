using System;
using Veldrid;

namespace Teraflop.Buffers {
	public interface IVertexBufferDescription {
		VertexLayoutDescription LayoutDescription { get; }
		uint SizeInBytes { get; }
	}

	internal class VertexLayoutHelpers {
		public static uint GetSizeInBytes(VertexElementFormat format) {
			switch (format) {
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

		public static int GetElementCount(VertexElementFormat format) {
			switch (format) {
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
