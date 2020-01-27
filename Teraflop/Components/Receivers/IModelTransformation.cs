using System.Collections.Generic;
using Teraflop.Components.Geometry;
using Veldrid;

namespace Teraflop.Components.Receivers
{
    public interface IModelTransformation
    {
        ModelTransformation ModelTransformation { set; }
    }

    public class ModelTransformation
    {
        public ModelTransformation(Transformation transformation)
        {
            Resources = transformation;
        }
        public IComposableResource Resources { get; }
    }
}
