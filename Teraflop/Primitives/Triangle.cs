using System.Numerics;
using Teraflop.Buffers;
using Teraflop.Buffers.Layouts;
using Teraflop.Components;

namespace Teraflop.Primitives {
	public class Triangle : IPrimitive {
		public Triangle(string name, bool doubleSided = false) {
			var builder = new MeshBuilder()
				.WithVertices(vertices).WithIndices(new ushort[] { 0, 1, 2 });
			if (doubleSided) {
				builder.WithIndices(new ushort[] { 0, 1, 2, 2, 1, 0 });
			}

			MeshData = builder.Build<VertexPositionNormal>(name);
		}
		public MeshData MeshData { get; private set; }

		private readonly IVertexBufferDescription[] vertices = new IVertexBufferDescription[]
		{
			new VertexPositionNormal(new Vector3(-0.5f, -0.5f, -0f), new Vector3(0, 0, 0)),
			new VertexPositionNormal(new Vector3(+0f, +0.5f, -0f), new Vector3(0.5f, 0, 0)),
			new VertexPositionNormal(new Vector3(+0.5f, -0.5f, +0f), new Vector3(0.5f, 0.5f, 0))
		};
	}
}
