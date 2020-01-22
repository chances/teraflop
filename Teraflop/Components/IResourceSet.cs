using System.Collections.Generic;

namespace Teraflop.Components
{
    public interface IResourceSet
    {
        IEnumerable<ResourceLayoutElementDescription> ResourceLayout { get; }
        void BindResourceSet();
    }
}
