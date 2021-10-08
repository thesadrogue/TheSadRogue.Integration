using System.Linq;
using GoRogue.GameFramework;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace TheSadRogue.Integration.Templates.MonoGame
{
    /// <summary>
    /// Simple component that moves its parent toward the player if the player is visible.
    /// </summary>
    internal class EnemyAI : RogueLikeComponentBase<RogueLikeEntity>
    {
        public EnemyAI()
            : base(false, false, false, false)
        { }

        public void TakeTurn()
        {
            if (Parent == null) return;
            if (Parent!.CurrentMap == null) return;
            if (!Parent.CurrentMap.PlayerFOV.CurrentFOV.Contains(Parent.Position)) return;

            var path = Parent.CurrentMap.AStar.ShortestPath(Parent.Position, Program.GameScreen.Player.Position);
            if (path == null) return;
            var firstPoint = path.GetStep(0);
            if (Parent.CanMove(firstPoint))
                Parent.Position = firstPoint;
        }
    }
}
