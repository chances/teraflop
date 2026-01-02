using System;
using System.Linq;
using System.Collections.Generic;
using Teraflop.Components;
using Veldrid;
using Teraflop.Components.Receivers;

namespace Teraflop.ECS {
	public class Composition : Resource, IResourceComposer, IBindableResource {
		private SortedList<string, IComposableResource> _resources =
			new SortedList<string, IComposableResource>();
		private Dictionary<Type, string> _resourceTypes = new Dictionary<Type, string>();

		protected Composition(string name, Type[] resourceOrder) : base(name) {
			for (int index = 0; index < resourceOrder.Length; index++) {
				Type resourceType = resourceOrder[index];
				string key = $"{index}:{resourceType.Name}";
				_resourceTypes.Add(resourceType, key);
			}

			Resources.OnInitialize += (_, e) => {
				var factory = e.ResourceFactory;

				var resources = _resources.Values;
				ResourceLayout = factory.CreateResourceLayout(new ResourceLayoutDescription(
					resources.SelectMany(resource => resource.ResourceLayout).ToArray()
				));
				ResourceSet = factory.CreateResourceSet(new ResourceSetDescription(
					ResourceLayout,
					resources.SelectMany(resource => resource.ResourceSet).ToArray()
				));
			};
		}

		public IEnumerable<Type> ComposableResourceTypes => _resourceTypes.Keys;
		public ResourceLayout ResourceLayout { get; private set; }
		public ResourceSet ResourceSet { get; private set; }
		public bool AreDependenciesSatisfied => _resources.Count == _resourceTypes.Keys.Count;

		/// <summary>
		/// Whether or not the given type is composable in this <see cref="Composition"/>.
		/// </summary>
		/// <typeparam name="T">Type to check for composability.</typeparam>
		public bool IsComposable<T>() => ComposableResourceTypes.Contains(typeof(T));

		/// <summary>
		/// Whether or not the given type is composable in this <see cref="Composition"/>.
		/// </summary>
		/// <param name="type">Type to check for composability.</param>
		public bool IsComposable(Type type) => ComposableResourceTypes.Contains(type);

		public void Compose(IComposableResource resource) {
			Type key = resource.GetType();
			if (!_resourceTypes.ContainsKey(key)) {
				var message = $"Resource may not be composed. {key.Name} does not appear in " +
					"this Composition's list of composable types.";
				throw new ArgumentException(message, nameof(resource));
			}

			if (_resources.ContainsKey(_resourceTypes[key])) {
				_resources[_resourceTypes[key]] = resource;
			} else {
				_resources.Add(_resourceTypes[key], resource);
			}
		}

		public static Composition Of<T>(string name) =>
			new Composition(name, new Type[] { typeof(T) });
		public static Composition Of<T, T2>(string name) =>
			new Composition(name, new Type[] { typeof(T), typeof(T2) });
		public static Composition Of<T, T2, T3>(string name) =>
			new Composition(name, new Type[] { typeof(T), typeof(T2), typeof(T3) });
		public static Composition Of<T, T2, T3, T4>(string name) =>
			new Composition(name, new Type[] {
				typeof(T), typeof(T2), typeof(T3), typeof(T4)
			});
		public static Composition Of<T, T2, T3, T4, T5>(string name) =>
			new Composition(name, new Type[] {
				typeof(T), typeof(T2), typeof(T3), typeof(T4), typeof(T5)
			});
	}
}
