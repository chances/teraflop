using System;
using Veldrid;

namespace Teraflop.Components
{
    // TODO: Rename this to something better? It's too similar to IResource, but it jives with Veldrid's Resource Set...?
    public interface IResourceSet
    {
        ResourceLayout ResourceLayout { get; }
        ResourceSet ResourceSet { get; }
    }
}
