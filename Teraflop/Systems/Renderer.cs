using System;
using System.Collections.Generic;
using System.Linq;
using Teraflop.Components;
using Teraflop.ECS;
using OpenTK.Graphics.ES20;
using OpenTK.Graphics;

namespace Teraflop.Systems
{
    public class Renderer : ECS.System
    {
        public Renderer(World world) : base(world)
        {
        }

        public override void Operate()
        {
            // TODO: Support second and more framebuffers
            GL.ClearColor(Color4.Black);
            GL.ClearDepth(1f);
            GL.ClearStencil(0);

            var renderables = World.Where(CanOperateOn)
                .GroupBy(entity =>
                {
                    var mesh = entity.GetComponent<MeshData>();
                    return (
                        entity.GetComponent<Material>(),
                        mesh.FrontFace,
                        mesh.PrimitiveTopology,
                        entity.GetComponent<IResourceLayout>().ResourceLayout,
                        mesh.VertexBuffer.LayoutDescription
                    );
                });
            foreach (var renderable in renderables)
            {
                var material = renderable.Key.Item1;
                var frontFace = renderable.Key.FrontFace;
                var primitiveTopology = renderable.Key.PrimitiveTopology;
                var resourceLayout = renderable.Key.ResourceLayout;
                var vertexLayout = renderable.Key.LayoutDescription;

                GL.BlendFunc(material.BlendStateSource, material.BlendStateDestination);
                GL.DepthMask(material.DepthStencilState.DepthWriteEnabled);
                GL.DepthFunc(material.DepthStencilState.DepthComparison);
                GL.CullFace(material.CullMode);
                GL.FrontFace(frontFace);
                // TODO: GL.Scissor() if we need to enable the scissor test
                GL.UseProgram(material.ShaderProgramHandle);

                var meshesWithUniforms = renderable.Select(entity => (
                    entity.GetComponent<MeshData>(),
                    entity.GetComponent<IResourceLayout>().ResourceLayout
                ));
                foreach (var meshAndUniforms in meshesWithUniforms)
                {
                    var mesh = meshAndUniforms.Item1;
                    var vertexBuffer = mesh.VertexBuffer;

                    GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer.VertexBufferHandle);
                    // Change this for mesh.Indices?
                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                    GL.EnableVertexAttribArray(0);

                    // TODO: Group renderables by MeshData and figure out instance uniforms

                    GL.DrawArrays(primitiveTopology, 0, mesh.Indices.Count);
                }
            }
        }

        /// <remarks>
        /// A renderable entity requires these components:
        /// <list type="bullet">
        /// <item>
        /// <description><see cref="Material"/></description>
        /// </item>
        /// <item>
        /// <description><see cref="MeshData"/></description>
        /// </item>
        /// <item>
        /// <description>and <see cref="IResourceLayout"/></description>
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="entity"></param>
        /// <returns>Whether this <see cref="Renderer"/> can operate on the given <see cref="Entity"/></returns>
        private static bool CanOperateOn(Entity entity) => entity.HasComponentsOfTypes
        (
            typeof(Material),
            typeof(MeshData),
            typeof(IResourceLayout)
        ) && entity.HasTag(Tags.Initialized)
        && entity.GetComponent<IResourceLayout>().ResourceLayout.Kind == ResourceKind.UniformBuffer;
    }
}
