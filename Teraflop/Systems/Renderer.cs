using System;
using System.Collections.Generic;
using System.Linq;
using Teraflop.Components;
using Teraflop.ECS;
using OpenTK.Graphics.ES30;
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
            GL.Clear(ClearBufferMask.ColorBufferBit |
                ClearBufferMask.DepthBufferBit |
                ClearBufferMask.StencilBufferBit);

            var renderables = World.Where(CanOperateOn)
                .GroupBy(entity =>
                {
                    var mesh = entity.GetComponent<MeshData>();
                    return (
                        entity.GetComponent<Material>(),
                        mesh.FrontFace,
                        mesh.PrimitiveTopology,
                        mesh.VertexBuffer.LayoutDescription
                    );
                });
            foreach (var renderable in renderables)
            {
                var material = renderable.Key.Item1;
                var frontFace = renderable.Key.FrontFace;
                var primitiveTopology = renderable.Key.PrimitiveTopology;
                var vertexLayout = renderable.Key.LayoutDescription;

                GL.BlendFunc(material.BlendStateSource, material.BlendStateDestination);
                GL.DepthMask(material.DepthStencilState.DepthWriteEnabled);
                GL.DepthFunc(material.DepthStencilState.DepthComparison);
                GL.CullFace(material.CullMode);
                GL.FrontFace(frontFace);
                // TODO: GL.Scissor() if we need to enable the scissor test
                GL.UseProgram(material.ShaderProgramHandle);

                var meshesWithResources = renderable.Select(entity => (
                    entity.GetComponent<MeshData>(),
                    entity.GetComponent<IResourceSet>()
                ));
                foreach (var meshAndResources in meshesWithResources)
                {
                    var mesh = meshAndResources.Item1;
                    var vertexBuffer = mesh.VertexBuffer;
                    var bufferLayout = vertexBuffer.LayoutDescription;

                    // Bind vertex data
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer.VertexBufferHandle);
                    // TODO: Change this for mesh.Indices?
                    for (int i = 0; i < bufferLayout.Elements.Length; i++)
                    {
                        var element = bufferLayout.Elements[i];
                        GL.VertexAttribPointer(i, (int) element.SizeInBytes,
                            // TODO: Convert element.Format to VertexAttribPointerType
                            VertexAttribPointerType.Float, false,
                            (int) bufferLayout.Stride, (int) element.Offset);
                        GL.EnableVertexAttribArray(i);
                        GL.BindAttribLocation(material.ShaderProgramHandle, i, element.Name);
                        GL.LinkProgram(material.ShaderProgramHandle);
                    }

                    // Bind uniforms
                    meshAndResources.Item2.BindResourceSet(material.ShaderProgramHandle);

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
        /// <description>and <see cref="IResourceSet"/></description>
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="entity"></param>
        /// <returns>Whether this <see cref="Renderer"/> can operate on the given <see cref="Entity"/></returns>
        private static bool CanOperateOn(Entity entity) => entity.HasComponentsOfTypes
        (
            typeof(Material),
            typeof(MeshData),
            typeof(IResourceSet)
        ) && entity.HasTag(Tags.Initialized);
    }
}
