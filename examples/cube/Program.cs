using System;
using System.Linq;
using Teraflop.Components;
using Teraflop.Components.Geometry;
using Teraflop.ECS;
using Teraflop.Entities;
using Veldrid;

namespace Teraflop.Examples.Triangle;

class Program {
    static void Main(string[] args) {
        new Cube().Run();
    }
}

internal class Cube : Example {
    public Cube() {
        // Print all assets in library
        foreach (var assetPath in AssetSources.SelectMany(source => source.AssetFilenames)) {
            Console.WriteLine(assetPath);
        }
    }

    protected override void Initialize() {
        Title = "Cube";

        World.Add(EntityFactory.Create(new OrbitCamera()));

        var flatMaterial = new Material("FlatMaterial", "flat.hlsl");
        World.Add(EntityFactory.Create(
            new Primitives.Cube("Cube").MeshData,
            flatMaterial,
            new Transformation(),
            new Color(RgbaFloat.CornflowerBlue),
            Composition.Of<Transformation, Color>("Cube")
        ));
    }
}
