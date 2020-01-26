using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using Teraflop.Assets;
using Teraflop.Buffers.Uniforms;
using Teraflop.Components;
using Teraflop.ECS;
using Teraflop.Entities;
using Veldrid;

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

    internal class Triangle : ExampleGame
    {
        protected override void Initialize()
        {
            Title = "Triangle";
            AssetDirectoryPaths.Add(AssetType.Shader, "triangle.Content.Shaders");

            var flatMaterial = new Material("FlatMaterial", "flat.hlsl");
            World.Add(EntityFactory.Create(
                new Primitives.Triangle("Triangle").MeshData,
                flatMaterial,
                new TriangleResource(RgbaFloat.Green)
            ));
        }
    }

    internal class TriangleResource : ResourceComponent, IResourceSet
    {
        private UniformModelTransformation _model =
            new UniformModelTransformation(Matrix4x4.Identity);
        private UniformModelTransformation _view =
            new UniformModelTransformation(Matrix4x4.Identity);
        private UniformColor _color = new UniformColor();

        public TriangleResource(RgbaFloat color) : base("Triangle")
        {
            Resources.OnInitialize = (factory, graphicsDevice) => {
                _model.Buffer.Initialize(factory, graphicsDevice);
                _view.Buffer.Initialize(factory, graphicsDevice);
                _color.Buffer.Initialize(factory, graphicsDevice);

                _color.Buffer.UniformData = color;

                ResourceLayout = factory.CreateResourceLayout(new ResourceLayoutDescription(
                    new ResourceLayoutElementDescription[] {
                        new ResourceLayoutElementDescription("Model", ResourceKind.UniformBuffer,
                            ShaderStages.Vertex),
                        new ResourceLayoutElementDescription("ViewProj", ResourceKind.UniformBuffer,
                            ShaderStages.Vertex),
                        new ResourceLayoutElementDescription("Color", ResourceKind.UniformBuffer,
                            ShaderStages.Fragment)
                    }
                ));

                ResourceSet = factory.CreateResourceSet(new ResourceSetDescription(
                    ResourceLayout,
                    _model.Buffer.DeviceBuffer,
                    _view.Buffer.DeviceBuffer,
                    _color.Buffer.DeviceBuffer
                ));
            };
            Resources.OnDispose = () => {
                _model.Buffer.Dispose();
                _view.Buffer.Dispose();
                _color.Buffer.Dispose();
            };
        }

        public ResourceLayout ResourceLayout { get; private set; }

        public ResourceSet ResourceSet { get; private set; }
    }
}
