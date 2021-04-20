using System.Collections.Generic;
using SadRogue.Primitives;

namespace SadRogue.Integration.FieldOfView
{
    /// <summary>
    /// Basic field of view handler that makes entities outside of FOV invisible, and makes
    /// terrain outside of FOV invisible if it is not explored, and grey-colored if it is
    /// explored.
    /// </summary>
    /// <remarks>
    /// This implementation is fairly basic, and does not support complex/relatively uncommon
    /// cases like changing foreground colors of cells while they are explored but outside
    /// of FOV.
    ///
    /// For cases like this, or cases where you want to handle FOV differently, feel free to
    /// create your own implementation of <see cref="FieldOfViewHandlerBase"/>.  This one
    /// may at least serve as an example, even if it does not fit your use case.
    /// </remarks>
    public class DefaultFieldOfViewHandler : FieldOfViewHandlerBase
    {
        /// <summary>
        /// Foreground color to set to all terrain that is outside of FOV but has been explored.
        /// </summary>
        public Color ExploredColor { get; }

        // Maps terrain to the foreground colors found before FOV visibility is applied.
        private readonly Dictionary<RogueLikeCell, Color> _originalForegroundColors;

        public DefaultFieldOfViewHandler(Color exploredColor, State startingState = State.Enabled)
            : base(startingState)
        {
            ExploredColor = exploredColor;
            _originalForegroundColors = new Dictionary<RogueLikeCell, Color>();
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
        /// Makes terrain visible and sets its foreground color to its regular value.
        /// </summary>
        /// <param name="terrain">Terrain to modify.</param>
        protected override void UpdateTerrainSeen(RogueLikeCell terrain)
        {
            terrain.Appearance.IsVisible = true;

            // If we've changed the color previously, put it back; otherwise, the original
            // color should still be in the foreground
            if (_originalForegroundColors.ContainsKey(terrain))
            {
                terrain.Appearance.Foreground = _originalForegroundColors[terrain];
                _originalForegroundColors.Remove(terrain);
            }
        }

        /// <summary>
        /// Makes terrain invisible if it is not explored.  Makes terrain visible but sets its foreground to
        /// <see cref="ExploredColor"/> if it is explored.
        /// </summary>
        /// <param name="terrain">Terrain to modify.</param>
        protected override void UpdateTerrainUnseen(RogueLikeCell terrain)
        {
            // Parent can't be null because of invariants enforced by structure for when
            // this function is called
            var parent = Parent!;
            if (parent.PlayerExplored[terrain.Position])
            {
                // Record old color in our list so we can restore it later
                _originalForegroundColors[terrain] = terrain.Appearance.Foreground;

                // Modify the foreground color to the darkened version
                terrain.Appearance.Foreground = ExploredColor;
            }
            else // If it isn't explored, it's invisible
                terrain.Appearance.IsVisible = false;
        }
    }
}
