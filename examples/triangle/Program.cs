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
                new Components.Color(RgbaFloat.Green),
                new Triangle()
            ));
        }
    }

    internal class Triangle : Resource, IModelTransformation, IColor, IDependencies, IBindableResource
    {
        public Triangle() : base(nameof(Triangle))
        {
            Resources.OnInitialize += (_, e) => {
                var factory = e.ResourceFactory;

                var transform = ModelTransformation.Resources;
                var color = Color.Resources;
                ResourceLayout = factory.CreateResourceLayout(new ResourceLayoutDescription(
                    transform.ResourceLayout.Concat(color.ResourceLayout).ToArray()
                ));
                ResourceSet = factory.CreateResourceSet(new ResourceSetDescription(
                    ResourceLayout,
                    transform.ResourceSet.Concat(color.ResourceSet).ToArray()
                ));
            };
        }

        public ModelTransformation ModelTransformation { private get; set; }
        public Components.Receivers.Color Color { private get; set; }
        public bool AreDependenciesSatisfied =>
          ModelTransformation != null && Color != null;

        public ResourceLayout ResourceLayout { get; private set; }
        public ResourceSet ResourceSet { get; private set; }
    }
}
