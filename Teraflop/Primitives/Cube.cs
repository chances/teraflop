using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Csg;
using Teraflop.Buffers;
using Teraflop.Buffers.Layouts;
using Teraflop.Components;

namespace Teraflop.Primitives
{
    public class Cube : IPrimitive
    {
        public Cube(string name)
        {
            var cube = Solids.Cube(1, true).Translate(0, 0.5);

            MeshData = MeshBuilder.FromSolid(cube, name);
        }

        public MeshData MeshData { get; private set; }
    }
}
