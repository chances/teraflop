using System;
using Veldrid;

namespace Teraflop.Components
{
    public class Resources : IResource
    {
        public Action OnInitialize { private get; set; }
        public Action OnDispose { private get; set; }
        public bool Initialized { get; private set; } = false;

        public void Initialize()
        {
            OnInitialize();
            Initialized = true;
        }

        public void Dispose()
        {
            OnDispose();
            Initialized = false;
        }
    }
}
