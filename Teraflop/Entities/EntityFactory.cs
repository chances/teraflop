using Teraflop.ECS;
using JetBrains.Annotations;

namespace Teraflop.Entities
{
    public static class EntityFactory
    {
        /// <summary>
        /// Create a new <see cref="Entity"/> containing all given <see cref="Component"/> instances.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public static Entity Create(params Component[] components) => new Entity(components);

        /// <summary>
        /// Create a new <see cref="Entity"/> containing only a single <see cref="Component"/> instance.
        /// </summary>
        /// <param name="name">Optionally rename the Component</param>
        /// <typeparam name="T">Specific subclass of <see cref="Component"/> to instantiate</typeparam>
        /// <returns></returns>
        public static Entity Create<T>([CanBeNull] string name = null) where T : Component, new()
        {
            var component = new T();
            if (name != null)
            {
                component.Name = name;
            }

            return new Entity(new[] {component});
        }
    }
}
