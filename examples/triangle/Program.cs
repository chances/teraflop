using System;
using System.Reflection;
using Teraflop.Assets;
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

    internal class Triangle : Example
    {
        protected override void Initialize()
        {
            Title = "Triangle";
            AssetDirectoryPaths.Add(AssetType.Shader, "triangle.Content.Shaders");

            World.Add(EntityFactory.Create(new OrbitCamera()));

            var flatMaterial = new Material("FlatMaterial", "flat.hlsl");
            World.Add(EntityFactory.Create(
                new Primitives.Triangle("Triangle", doubleSided: true).MeshData,
                flatMaterial,
                new Transformation(),
                new Color(RgbaFloat.Green),
                Composition.Of<Transformation, Color>("Triangle")
            ));
        }
    }
}
