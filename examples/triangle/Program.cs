using System;
using System.Linq;
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
            new Triangle().Run();
        }
    }

    internal class Triangle : Example
    {
        public Triangle()
        {
            // Print all assets in library
            foreach (var assetPath in AssetSources.SelectMany(source => source.AssetFilenames))
            {
                Console.WriteLine(assetPath);
            }
        }

        protected override void Initialize()
        {
            Title = "Triangle";

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
