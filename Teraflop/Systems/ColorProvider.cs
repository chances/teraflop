using System.Linq;
using Teraflop.Components.Receivers;
using Teraflop.ECS;

namespace Teraflop.Systems
{
    public class ColorProvider : System<IColor>
    {
        public ColorProvider(World world) : base(world)
        {
        }

        public override void Operate()
        {
            foreach (var model in OperableEntities.Where(entity => entity.HasComponent<Components.Color>()))
            {
                var color = model.GetComponent<Components.Color>();
                if (color.Initialized) {
                    model.GetComponent<IColor>().Color = new Components.Receivers.Color(color);
                }
            }
        }
    }
}
