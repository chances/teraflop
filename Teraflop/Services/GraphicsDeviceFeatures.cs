using Veldrid;

namespace Teraflop.Services
{
    public class GraphicsDeviceFeatures
    {
        private GraphicsDevice _device;

        public GraphicsDeviceFeatures(GraphicsDevice device)
        {
            _device = device;
        }

        public GraphicsBackend BackendType => _device.BackendType;
        public Veldrid.GraphicsDeviceFeatures Features => _device.Features;
    }
}
