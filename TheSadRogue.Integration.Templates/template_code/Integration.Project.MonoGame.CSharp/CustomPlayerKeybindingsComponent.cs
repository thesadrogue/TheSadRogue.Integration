using GoRogue.GameFramework;
using SadRogue.Integration.Keybindings;
using SadRogue.Primitives;

namespace TheSadRogue.Integration.Templates.MonoGame
{
    /// <summary>
    /// Subclass of the integration library's keybindings component that moves enemies as appropriate when the player
    /// moves.
    /// </summary>
    /// <remarks>
    /// Components can also be attached to maps, so the code for calling TakeTurn on all entities could be moved to a
    /// map component as well so that it is re-usable.
    /// </remarks>
    internal class CustomPlayerKeybindingsComponent : PlayerKeybindingsComponent
    {
        protected override void MotionHandler(Direction direction)
        {
            if (!Parent!.CanMoveIn(direction)) return;

            Parent.Position += direction;

            foreach (var entity in Parent.CurrentMap!.Entities.Items)
            {
                var ai = entity.GoRogueComponents.GetFirstOrDefault<EnemyAI>();
                ai?.TakeTurn();
            }
        }
    }
}
