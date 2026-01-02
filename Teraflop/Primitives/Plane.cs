using System.Collections.Generic;
using System.Numerics;
using Teraflop.Buffers;
using Teraflop.Buffers.Layouts;
using Teraflop.Components;

namespace Teraflop.Primitives {
	/// <summary>
	/// A 2D plane with an upwards facing normal.
	/// </summary>
    public class Plane : IPrimitive {
        public Plane(string name) {
            var mesh = new MeshBuilder().WithVertices(new List<IVertexBufferDescription> {
                new VertexPositionNormalTexture(new Vector3(-1, -1, 0), new Vector3(0, 0, 1), new Vector2(0, 0)),
                new VertexPositionNormalTexture(new Vector3(-1, 1, 0), new Vector3(0, 0, 1), new Vector2(0, 1)),
                new VertexPositionNormalTexture(new Vector3(1, 1, 0), new Vector3(0, 0, 1), new Vector2(1, 1)),
                new VertexPositionNormalTexture(new Vector3(1, -1, 0), new Vector3(0, 0, 1), new Vector2(1, 0))
			}).WithIndices(new ushort[] { 0, 1, 2, 0, 2, 3 });

            MeshData = mesh.Build<VertexPositionNormalTexture>(name);
        }

        public MeshData MeshData { get; private set; }
    }
}
