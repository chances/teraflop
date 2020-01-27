namespace Teraflop.Components.Receivers
{
    public interface IColor
    {
        Color Color { set; }
    }

    public class Color
    {
        public Color(Components.Color color)
        {
            Resources = color;
        }
        public IComposableResource Resources { get; }
    }
}
