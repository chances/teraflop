using Veldrid;

namespace Teraflop.Components
{
    public interface IBindableResource
    {
        ResourceLayout ResourceLayout { get; }
        ResourceSet ResourceSet { get; }
    }
}
