using System;
using System.Collections.Generic;
using System.Linq;
using Teraflop.Components;
using Teraflop.ECS;
using Veldrid;
using InitializeAction = System.Action;

namespace Teraflop.Systems
{
    public class ResourceInitializer : ECS.System
    {
        public ResourceInitializer(World world) : base(world)
        {
        }

        private new IEnumerable<Entity> OperableEntities => World
            .Where(entity => {
                var hasResources = (entity.HasComponent<IResource>());
                var isEntityInitialized = entity.HasTag(Tags.Initialized);
                var areAnyResourcesUninitialized = entity.HasComponent<IResource>() &&
                    entity.GetComponents<IResource>().Any(component => !component.Initialized);

                return hasResources && (!isEntityInitialized || areAnyResourcesUninitialized);
            });

        private Dictionary<Entity, IEnumerable<InitializeAction>> OperableEntityInitializers =>
            OperableEntities.ToDictionary(
                entity => entity,
                entity => entity.GetComponents<IResource>().Select<IResource, InitializeAction>(resource =>
            {
                // Only init device resources when dependencies are satisfied
                if (resource is IDependencies dependant && !dependant.AreDependenciesSatisfied)
                {
                    return () => {}; // No-op
                }

                return resource.Initialize;
            }));

        public override void Operate()
        {
            foreach (var entityAndInitializers in OperableEntityInitializers)
            {
                var entity = entityAndInitializers.Key;

                foreach (var initializeAction in entityAndInitializers.Value)
                {
                    initializeAction.Invoke();
                }

                var isEntityInitialized = entity.Values.OfType<IResource>()
                    .Aggregate(true, (isInitialized, resource) => isInitialized && resource.Initialized);
                if (isEntityInitialized)
                {
                    entity.AddTag(Tags.Initialized);
                }
            }
        }
    }
}
