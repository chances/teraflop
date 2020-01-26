using System;
using System.Linq;
using System.Reflection;
using Teraflop.Assets;
using Teraflop.Buffers.Uniforms;
using Teraflop.Components;
using Teraflop.Components.Geometry;
using Teraflop.Components.Receivers;
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

            new TriangleExample().Run();
        }
    }

    internal class TriangleExample : ExampleGame
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
                new Transformation(),
                new Triangle(RgbaFloat.Green)
            ));
        }
    }

    internal class Triangle : ResourceComponent, IModelTransformation, IDependencies, IResourceSet
    {
        private UniformColor _color = new UniformColor();

        public Triangle(RgbaFloat color) : base(nameof(Triangle))
        {
            Resources.OnInitialize += (_, e) => {
                var factory = e.ResourceFactory;
                _color.Buffer.Initialize(e.ResourceFactory, e.GraphicsDevice);
                _color.Buffer.UniformData = color;

                ResourceLayout = factory.CreateResourceLayout(new ResourceLayoutDescription(
                    ModelTransformation.ResourceLayout.Append(_color.LayoutDescription).ToArray()
                ));

                ResourceSet = factory.CreateResourceSet(new ResourceSetDescription(
                    ResourceLayout,
                    ModelTransformation.ResourceSet.Append(_color.Buffer.DeviceBuffer).ToArray()
                ));
            };
            Resources.OnDispose += (_, __) => {
                _color.Buffer.Dispose();
            };
        }

        public ModelTransformation ModelTransformation { private get; set; }
        public bool AreDependenciesSatisfied => ModelTransformation != null;

        public ResourceLayout ResourceLayout { get; private set; }
        public ResourceSet ResourceSet { get; private set; }
    }
}
