using System.Linq;
using Teraflop.Components.Geometry;
using Teraflop.Components.Receivers;
using Teraflop.ECS;

namespace Teraflop.Systems
{
    public class ModelTransformationProvider : System<IModelTransformation>
    {
        public ModelTransformationProvider(World world) : base(world)
        {
        }

        public override void Operate()
        {
            foreach (var model in OperableEntities.Where(entity => entity.HasComponent<Transformation>()))
            {
                var transformation = model.GetComponent<Transformation>().Value;
                model.GetComponent<IModelTransformation>().ModelTransformation = transformation;
            }
        }
    }
}
