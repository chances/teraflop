using Teraflop.Components;
using JetBrains.Annotations;
using Veldrid;

namespace Teraflop.ECS
{
    public abstract class Component
    {
        protected Component([CanBeNull] string name = null)
        {
            Name = name ?? this.GetType().Name;
        }

        public string Name { get; set; }
    }

    public abstract class ResourceComponent : Component, IResource
    {
        protected ResourceComponent(string name) : base(name)
        {
        }

        protected Resources Resources { get; } = new Resources();

        public bool Initialized => Resources.Initialized;

        public void Initialize(ResourceFactory factory, GraphicsDevice device) =>
            Resources.Initialize(factory, device);

        public void Dispose()
        {
            Resources.Dispose();
        }
    }
}
