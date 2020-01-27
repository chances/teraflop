namespace Teraflop.Components.Receivers
{
    public interface IResourceComposer : IDependencies
    {
        void Compose(IComposableResource resource);
    }
}
