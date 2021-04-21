using System;
using SadRogue.Primitives;

namespace SadRogue.Integration.FieldOfView
{
    /// <summary>
    /// Field of view handler that implements the concept of "memory", wherein the player sees previously
    /// seen tiles as they were when they were last in view until they come into view again.
    /// </summary>
    /// <remarks>
    /// All terrain objects in the map this component is added to must be of type <see cref="MemoryAwareRogueLikeCell"/>.
    /// If they are not, it will result in an exception being thrown at run-time.
    /// </remarks>
    public class MemoryFieldOfViewHandler : FieldOfViewHandlerBase
    {
        /// <summary>
        /// Color to use for the foreground of tiles that are outside of FOV but previously seen by
        /// the player.
        /// </summary>
        public Color ExploredColor { get; }

        /// <summary>
        /// Creates a new handler.
        /// </summary>
        /// <param name="exploredColor">
        /// Color to use for the foreground of tiles that are outside of FOV but previously seen by
        /// the player.
        /// </param>
        /// <param name="startingState">The starting value for <see cref="FieldOfViewHandlerBase.CurrentState"/>.</param>
        public MemoryFieldOfViewHandler(Color exploredColor, State startingState = State.Enabled)
            : base(startingState)
        {
            ExploredColor = exploredColor;
        }

        /// <summary>
        /// Makes entity visible.
        /// </summary>
        /// <param name="entity">Entity to modify.</param>
        protected override void UpdateEntitySeen(RogueLikeEntity entity) => entity.IsVisible = true;

        /// <summary>
        /// Makes entity invisible.
        /// </summary>
        /// <param name="entity">Entity to modify.</param>
        protected override void UpdateEntityUnseen(RogueLikeEntity entity) => entity.IsVisible = false;

        /// <summary>
        /// Makes terrain visible and sets its appearance to its regular value, and ensures that it will
        /// remain as such as long as it is visible.
        /// </summary>
        /// <param name="terrain">Terrain to modify.</param>
        protected override void UpdateTerrainSeen(RogueLikeCell terrain)
        {
            // Invalid use of MemoryFieldOfViewHandler
            if (!(terrain is MemoryAwareRogueLikeCell awareTerrain))
                throw new InvalidOperationException(
                    $"{nameof(MemoryFieldOfViewHandler)} must only be used on a  map that contains {nameof(MemoryAwareRogueLikeCell)} instances as its terrain.");

            // If the appearances don't match currently, synchronize them
            if (!awareTerrain.LastSeenAppearance.Matches(awareTerrain.TrueAppearance))
            {
                awareTerrain.LastSeenAppearance.CopyAppearanceFrom(awareTerrain.TrueAppearance);
                awareTerrain.LastSeenAppearance.IsVisible = awareTerrain.TrueAppearance.IsVisible;
            }


            // Set up an event handler that will keep the actual appearance in sync with the
            // true one, so long as the tile remains visible to the player.  Because this could be
            // called twice in certain state changes, we make sure we do not add the handler twice.
            //
            // Removing the event even if it does not exist does not throw exception and appears to be the
            // most performant way to account for duplicates.
            awareTerrain.TrueAppearance.IsDirtySet -= On_VisibleTileTrueAppearanceIsDirtySet;
            awareTerrain.TrueAppearance.IsDirtySet += On_VisibleTileTrueAppearanceIsDirtySet;
        }

        /// <summary>
        /// Makes terrain invisible if it is not explored.  Makes terrain visible but darkens the foreground
        /// color to <see cref="ExploredColor"/> if it is explored.  The terrain will continue to appear as
        /// it was last seen (even if its true appearance changes) until the player sees it again.
        /// </summary>
        /// <param name="terrain">Terrain to modify.</param>
        protected override void UpdateTerrainUnseen(RogueLikeCell terrain)
        {
            // Invalid use of MemoryFieldOfViewHandler
            if (!(terrain is MemoryAwareRogueLikeCell awareTerrain))
                throw new InvalidOperationException(
                    $"{nameof(MemoryFieldOfViewHandler)} must only be used on a  map that contains {nameof(MemoryAwareRogueLikeCell)} instances as its terrain.");

            // If the event handler for synchronizing the appearance with true appearance is added, remove it
            // which will cause the appearance to remain as last-seen if it changes
            awareTerrain.TrueAppearance.IsDirtySet -= On_VisibleTileTrueAppearanceIsDirtySet;

            // If the unseen terrain is explored (eg. it was in FOV previously), apply the darkened color
            // as appropriate.
            if (Parent!.PlayerExplored[terrain.Position])
                awareTerrain.LastSeenAppearance.Foreground = ExploredColor;
            else // If the unseen tile isn't explored, it's invisible
                awareTerrain.LastSeenAppearance.IsVisible = false;
        }

        private void On_VisibleTileTrueAppearanceIsDirtySet(object? sender, EventArgs e)
        {
            // Sender will not be null because of event invariants.  Cast is safe since we
            // control what this handler is added to and it is checked first
            var awareTerrain = (MemoryAwareRogueLikeCell)(((TerrainAppearance)sender!).Terrain);

            // If appearances are synchronized, there is nothing to do
            if (awareTerrain.LastSeenAppearance.Matches(awareTerrain.TrueAppearance))
                return;

            // Otherwise, synchronize them
            awareTerrain.LastSeenAppearance.CopyAppearanceFrom(awareTerrain.TrueAppearance);
            awareTerrain.LastSeenAppearance.IsVisible = awareTerrain.TrueAppearance.IsVisible;
        }
    }
}
