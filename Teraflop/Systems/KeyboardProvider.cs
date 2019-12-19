using Teraflop.Components.Receivers;
using Teraflop.ECS;
using Teraflop.Input;

namespace Teraflop.Systems
{
    public class KeyboardProvider : System<IKeyboardInput>
    {
        private KeyboardState _keyboardState;

        public KeyboardProvider(World world, KeyboardState keyboardState) : base(world)
        {
            _keyboardState = keyboardState;
        }

        public override void Operate()
        {
            foreach (var componentToUpdate in OperableComponents)
            {
                componentToUpdate.KeyboardState = _keyboardState;
            }
        }
    }
}
