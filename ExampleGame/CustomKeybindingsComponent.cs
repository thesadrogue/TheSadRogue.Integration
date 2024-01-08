using GoRogue.GameFramework;
using SadRogue.Integration;
using SadRogue.Integration.Keybindings;

namespace ExampleGame
{
    internal class CustomKeybindingsComponent : KeybindingsComponent<RogueLikeEntity>
    {
        protected override void MotionHandler(Direction direction)
        {
            if (Parent!.CanMoveIn(direction))
                Parent!.Position += direction;
        }
    }
}
