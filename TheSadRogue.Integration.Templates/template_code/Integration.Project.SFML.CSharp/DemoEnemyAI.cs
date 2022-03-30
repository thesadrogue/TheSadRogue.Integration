
using System.Linq;
using GoRogue.GameFramework;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using SadRogue.Primitives;

namespace TheSadRogue.Integration.Templates.SFML
{
    /// <summary>
    /// Simple component that moves its parent toward the player if the player is visible. It demonstrates the basic
    /// usage of the integration library's component system, as well as basic AStar pathfinding.
    /// </summary>
    internal class DemoEnemyAI : RogueLikeComponentBase<RogueLikeEntity>
    {
        public DemoEnemyAI()
            : base(false, false, false, false)
        { }

        public void TakeTurn()
        {
            if (Parent?.CurrentMap == null) return;
            if (!Parent.CurrentMap.PlayerFOV.CurrentFOV.Contains(Parent.Position)) return;

            var path = Parent.CurrentMap.AStar.ShortestPath(Parent.Position, Program.GameScreen.Player.Position);
            if (path == null) return;
            var firstPoint = path.GetStep(0);
            if (Parent.CanMove(firstPoint))
            {
                Program.GameScreen.MessageLog.AddMessage($"An enemy moves {Direction.GetDirection(Parent.Position, firstPoint)}!");
                Parent.Position = firstPoint;
            }

        }
    }
}
