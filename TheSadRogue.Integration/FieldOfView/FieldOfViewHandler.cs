using SadRogue.Primitives;

namespace TheSadRogue.Integration.FieldOfView
{
    /// <summary>
    /// Takes care of field of view and visibility
    /// </summary>
    public class FieldOfViewHandler : IFieldOfViewHandler
    {
        /// <inheritdoc />
        public bool Enabled { get; private set; }
        
        /// <summary>
        /// The map against which to work
        /// </summary>
        public RogueLikeMap Map { get; }
        
        /// <inheritdoc />
        public FieldOfViewState State { get; private set; }
        
        /// <inheritdoc />
        public Color ExploredForegroundColor { get; }
        
        /// <inheritdoc />
        public Color ExploredBackgroundColor { get; }

        /// <summary>
        /// Creates a new FieldOfViewHandler
        /// </summary>
        /// <param name="map">The map against which to work</param>
        /// <param name="exploredForegroundColor">The color of explored tiles that are not visible</param>
        /// <param name="exploredBackgroundColor">The background color of explored tiles that</param>
        /// <param name="state">The state of the visibility handler</param>
        public FieldOfViewHandler(RogueLikeMap map, Color exploredForegroundColor, Color exploredBackgroundColor,
            FieldOfViewState state = FieldOfViewState.Enabled)
        {
            Map = map;
            ExploredForegroundColor = exploredForegroundColor;
            ExploredBackgroundColor = exploredBackgroundColor;
            State = state;
        }
        
        /// <inheritdoc />
        public void SetState(FieldOfViewState state)
        {
            State = state;
            if(state == FieldOfViewState.Enabled)
                Enable();
            else
                Disable(false);
        }

        /// <inheritdoc />
        public void Enable()
        {
            Enabled = true;
        }

        /// <inheritdoc />
        public void Disable(bool resetVisibility)
        {
            Enabled = false;
        }
        
        /// <inheritdoc />
        public void UpdateTerrainSeen(RogueLikeEntity terrain)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void UpdateTerrainUnseen(RogueLikeEntity terrain)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void UpdateEntitySeen(RogueLikeEntity terrain)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void UpdateEntityUnseen(RogueLikeEntity terrain)
        {
            throw new System.NotImplementedException();
        }
    }
}