using System;
using Veldrid;

namespace Teraflop.Components
{
    public interface IResource : IDisposable
    {
        bool Initialized { get; }
        void Initialize();
    }
}
