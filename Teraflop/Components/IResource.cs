using System;

namespace Teraflop.Components
{
    public interface IResource : IDisposable
    {
        bool Initialized { get; }
        void Initialize();
    }
}
