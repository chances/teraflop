using System.Collections.Generic;
using System.Linq;
using Teraflop.Components.Receivers;
using Teraflop.ECS;
using Teraflop.Input;

namespace Teraflop.Systems
{
    public class InputProvider : ECS.System
    {
        private MouseState _mouseState;
        private KeyboardState _keyboardState;

        public InputProvider(World world, MouseState mouseState, KeyboardState keyboardState) : base(world)
        {
            _mouseState = mouseState;
            _keyboardState = keyboardState;
        }

        protected new IEnumerable<Entity> OperableEntities => World.Where(CanOperateOn);

        public override void Operate()
        {
            foreach (var component in OperableComponents)
            {
                if (component is IMouseInput mouseInputReceiver)
                {
                    mouseInputReceiver.MouseState = _mouseState;
                }
                if (component is IKeyboardInput keyboardInputReceiver)
                {
                    keyboardInputReceiver.KeyboardState = _keyboardState;
                }
            }
        }

        private static bool CanOperateOn(Entity entity) =>
            entity.HasComponent<IMouseInput>() || entity.HasComponent<IKeyboardInput>();
    }
}
