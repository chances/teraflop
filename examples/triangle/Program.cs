using System;
using System.Linq;
using System.Reflection;
using Teraflop.Assets;
using Teraflop.Buffers.Uniforms;
using Teraflop.Components;
using Teraflop.Components.Geometry;
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

            World.Add(EntityFactory.Create(new Camera()));

            var flatMaterial = new Material("FlatMaterial", "flat.hlsl");
            World.Add(EntityFactory.Create(
                new Primitives.Triangle("Triangle").MeshData,
                flatMaterial,
                new TriangleResource(RgbaFloat.Green)
            ));
        }
    }

    internal class TriangleResource : Transformation, IResourceSet
    {
        private UniformColor _color = new UniformColor();

        public TriangleResource(RgbaFloat color)
        {
            Resources.OnInitialize += (_, e) => {
                var factory = e.ResourceFactory;
                _color.Buffer.Initialize(e.ResourceFactory, e.GraphicsDevice);
                _color.Buffer.UniformData = color;

                ResourceLayout = factory.CreateResourceLayout(new ResourceLayoutDescription(
                    ResourceLayoutElements.Append(
                        new ResourceLayoutElementDescription("Color", ResourceKind.UniformBuffer,
                            ShaderStages.Fragment)
                    ).ToArray()
                ));

                ResourceSet = factory.CreateResourceSet(new ResourceSetDescription(
                    ResourceLayout,
                    ModelTransformation.DeviceBuffer,
                    CameraViewProjection.DeviceBuffer,
                    _color.Buffer.DeviceBuffer
                ));
            };
            Resources.OnDispose += (_, __) => {
                _color.Buffer.Dispose();
            };
        }

        public ResourceLayout ResourceLayout { get; private set; }

        public ResourceSet ResourceSet { get; private set; }
    }
}
