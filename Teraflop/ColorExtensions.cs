using System.Numerics;

namespace Teraflop
{
    public static class ColorExtentions
    {
        public static Vector4 ToVector4Argb(this OpenTK.Graphics.Color4 color) =>
            new Vector4(color.A, color.R, color.G, color.B);
    }
}
