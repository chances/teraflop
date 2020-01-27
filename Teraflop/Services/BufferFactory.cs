using Veldrid;

namespace Teraflop.Services
{
    public abstract class ResourceFactory
    {
        protected Veldrid.ResourceFactory _factory;

        public ResourceFactory(Veldrid.ResourceFactory factory)
        {
            _factory = factory;
        }
    }

    public class BufferFactory : ResourceFactory
    {
        public BufferFactory(Veldrid.ResourceFactory factory) : base(factory)
        {
        }

        public DeviceBuffer Create(BufferDescription description) =>
            _factory.CreateBuffer(description);
    }
}
