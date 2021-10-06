using System;
using GoRogue.Components.ParentAware;
using GoRogue.GameFramework;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using SadRogue.Primitives;

namespace TheSadRogue.Integration.Templates.MonoGame
{
    /// <summary>
    /// Component that you can attach to your player object to ensure that it re-calculates the map's FOV whenever the
    /// object is moved.  This could be also be achieved via a subclass of RogueLikeEntity; however the integration library
    /// makes it quick and easy to create custom components that function in both SadConsole's and GoRogue's component systems.
    /// </summary>
    internal class PlayerFOVController : RogueLikeComponentBase<RogueLikeEntity>
    {
        /// <summary>
        /// The sight radius of the player.
        /// </summary>
        public int FOVRadius { get; init; } = 10;

        public PlayerFOVController()
            : base(false, false, false, false)
        {
            // When the component is attached/detached from an object, set/reset hooks so that FOV is recalculated when
            // the object moves
            Added += OnAdded;
            Removed += OnRemoved;
        }

        /// <summary>
        /// Calculate player FOV if the parent is part of a map.
        /// </summary>
        public void CalculateFOV()
            => Parent?.CurrentMap?.PlayerFOV.Calculate(Parent.Position, FOVRadius, Parent.CurrentMap.DistanceMeasurement);

        private void OnAdded(object s, EventArgs e) => Parent!.Moved += OnMoved;

        private void OnRemoved(object s, ParentAwareComponentRemovedEventArgs<RogueLikeEntity> e)
            => e.OldParent.Moved -= OnMoved;

        private void OnMoved(object sender, GameObjectPropertyChanged<Point> e)
            => CalculateFOV();
    }
}
