using JetBrains.Annotations;

namespace Teraflop.ECS {
	public abstract class Component {
		protected Component([CanBeNull] string name = null) {
			Name = name ?? this.GetType().Name;
		}

		public string Name { get; set; }
	}
}
