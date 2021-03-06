using Teraflop.ECS;
using Veldrid;

namespace Teraflop.Systems
{
    public class ComponentUpdater : RealTimeSystem
    {
        public ComponentUpdater(World world) : base(world)
        {
        }

        public override void Operate(GameTime gameTime)
        {
            foreach (var componentToUpdate in OperableComponents)
            {
                componentToUpdate.Update(gameTime);
            }
        }
    }
}
