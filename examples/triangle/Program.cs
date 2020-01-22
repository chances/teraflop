using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using OpenTK.Graphics;
using Teraflop.Assets;
using Teraflop.Buffers.Uniforms;
using Teraflop.Components;
using Teraflop.ECS;
using Teraflop.Entities;

namespace Teraflop.Examples.Triangle
{
    class Program
    {
        static void Main(string[] args)
        {
            // Print all embedded resources
            foreach (var resourceName in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                Console.WriteLine(resourceName);
            }

            new Triangle().Run();
        }
    }

    internal class Triangle : Game
    {
        public Triangle() : base(title: "Triangle")
        {
            AssetDirectoryPaths.Add(AssetType.Shader, "triangle.Content.Shaders");
        }

        protected override void Initialize()
        {
            base.Initialize();

            var flatMaterial = new Material("FlatMaterial", "flat.hlsl");
            World.Add(EntityFactory.Create(
                new Primitives.Triangle("Triangle").MeshData,
                flatMaterial,
                new TriangleResource(Color4.Green)
            ));
        }

        protected override void Render(GameTime gameTime)
        {
            base.Render(gameTime);

            var frameTime = Math.Floor(gameTime.ElapsedGameTime.TotalMilliseconds);
            Title = $"Triangle - {frameTime} ms - {FramesPerSecond} fps";
        }
    }

    internal class TriangleResource : ResourceComponent, IResourceSet
    {
        private UniformModelTransformation _model =
            new UniformModelTransformation(Matrix4x4.Identity);
        private UniformModelTransformation _view =
            new UniformModelTransformation(Matrix4x4.Identity);
        private UniformColor _color = new UniformColor();

        public TriangleResource(Color4 color) : base("Triangle")
        {
            Resources.OnInitialize = () => {
                _model.Buffer.Initialize();
                _view.Buffer.Initialize();
                _color.Buffer.Initialize();

                _color.Buffer.UniformData = color;
            };
            Resources.OnDispose = () => {
                _model.Buffer.Dispose();
                _view.Buffer.Dispose();
                _color.Buffer.Dispose();
            };
        }

        public IEnumerable<ResourceLayoutElementDescription> ResourceLayout =>
            new ResourceLayoutElementDescription[] {
                new ResourceLayoutElementDescription("Model", ResourceKind.UniformBuffer,
                    ShaderStages.Vertex),
                new ResourceLayoutElementDescription("ViewProj", ResourceKind.UniformBuffer,
                    ShaderStages.Vertex),
                new ResourceLayoutElementDescription("Color", ResourceKind.UniformBuffer,
                    ShaderStages.Fragment)
            };
    }
}
