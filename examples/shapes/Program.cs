using System;
using System.Linq;
using System.Numerics;
using Teraflop.Components;
using Teraflop.Components.Geometry;
using Teraflop.Components.UI;
using Teraflop.ECS;
using Teraflop.Entities;
using Veldrid;

namespace Teraflop.Examples.Triangle;

class Program {
    static void Main(string[] args) {
        new Shapes().Run();
    }
}

internal class Shapes : Example {
    public Shapes() {
        // Print all assets in library
        foreach (var assetPath in AssetSources.SelectMany(source => source.AssetFilenames)) {
            Console.WriteLine(assetPath);
        }
    }

    protected override void Initialize() {
        Title = nameof(Shapes);

        World.Add(EntityFactory.Create(new OrbitCamera()));

		WebView.InitializeRenderer(Title);
		var webView = new WebView();
		World.Add(EntityFactory.Create(
			webView,
			new Surface(webView),
			Surface.Mesh
		));

        var flatMaterial = new Material("FlatMaterial", "flat.hlsl");

		World.Add(EntityFactory.Create(
			new Primitives.Plane("Ground").MeshData,
			flatMaterial,
			new Transformation() {
				Scale = new Vector3(new Vector2(50), 0)
			},
			new Color(RgbaFloat.Grey),
			Composition.Of<Transformation, Color>("Ground")
		));

		var left = new Transformation() { Translation = new Vector3(-2, 0, 0.5f) };
		World.Add(EntityFactory.Create(
            new Primitives.Cube("Cube").MeshData,
            flatMaterial,
            left,
            new Color(RgbaFloat.CornflowerBlue),
            Composition.Of<Transformation, Color>("Cube")
        ));

		var right = new Transformation() { Translation = new Vector3(1.5f, 0, 0.25f) };
		right.Scale *= new Vector3(0.5f);
		World.Add(EntityFactory.Create(
			new Primitives.Cube("Cube").MeshData,
			flatMaterial,
			right,
			new Color(RgbaFloat.Orange),
			Composition.Of<Transformation, Color>("Cube")
		));
	}
}
