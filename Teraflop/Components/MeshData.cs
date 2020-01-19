using System.Numerics;
using Teraflop.Buffers;
using Teraflop.ECS;
using JetBrains.Annotations;
using OpenTK.Graphics.ES20;

namespace Teraflop.Components
{
    public class MeshData : Component
    {
        public Vector3 Center { get; }
        // TODO: OpenTk Bounding box
        // public BoundingBox BoundingBox { get; }
        public PrimitiveType PrimitiveTopology { get; protected set; }
        public FrontFaceDirection FrontFace { get; protected set; }
        public VertexBuffer VertexBuffer { get; protected set; }
        public IndexBuffer Indices => VertexBuffer.Indices;

        public MeshData([CanBeNull] string name,
            PrimitiveType primitiveTopology = PrimitiveType.Triangles,
            FrontFaceDirection frontFace = FrontFaceDirection.Cw) : base(name)
        {
            PrimitiveTopology = primitiveTopology;
            FrontFace = frontFace;
        }
    }

    public class MeshData<T> : MeshData, IResource where T : struct, IVertexBufferDescription
    {
        public bool Initialized => VertexBuffer.Initialized;

        public MeshData(string name,
            VertexBuffer<T> vertexBuffer,
            PrimitiveType primitiveTopology = PrimitiveType.Triangles,
            FrontFaceDirection frontFace = FrontFaceDirection.Cw) : base(name)
        {
            VertexBuffer = vertexBuffer;
            PrimitiveTopology = primitiveTopology;
            FrontFace = frontFace;
        }

        public void Initialize()
        {
            (VertexBuffer as VertexBuffer<T>).Initialize();
            VertexBuffer.Name = $"{Name} VBO";
            Indices.Name = $"{Name} IBO";
        }

        public void Dispose()
        {
            (VertexBuffer as VertexBuffer<T>).Dispose();
        }
    }
}
