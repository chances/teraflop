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
            Transformation = transformation;
            ResourceLayout = transformation.ResourceLayoutElements;
            ResourceSet = transformation.ResourceSet;
        }
        public Transformation Transformation { get; }
        public IEnumerable<ResourceLayoutElementDescription> ResourceLayout { get; }
        public IEnumerable<BindableResource> ResourceSet { get; }
    }
}
