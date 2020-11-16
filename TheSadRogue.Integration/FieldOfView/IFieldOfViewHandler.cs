using SadRogue.Primitives;

namespace TheSadRogue.Integration.FieldOfView
{
    /// <summary>
    /// Handles Field of View and Visibility. (Should this be a component?)
    /// </summary>
    public interface IFieldOfViewHandler
    {
        /// <summary>
        /// Whether or not to calculate Visibility
        /// </summary>
        bool Enabled { get; }
        
        /// <summary>
        /// The map on which to calculate
        /// </summary>
        RogueLikeMap Map { get; }
        
        
        /// <summary>
        /// The state of the handler
        /// </summary>
        FieldOfViewState State { get; }
        
        /// <summary>
        /// the color to set the "explored" terrain to when it leaves visibility
        /// </summary>
        Color ExploredForegroundColor { get; }
        
        /// <summary>
        /// the color to set the "explored" terrain to when it leaves visibility
        /// </summary>
        Color ExploredBackgroundColor { get; }
        
        /// <summary>
        /// Changes the current state of this visibility handler
        /// </summary>
        /// <param name="state">The state to set</param>
        void SetState(FieldOfViewState state);
        
        /// <summary>
        /// Begin calculating visibility
        /// </summary>
        void Enable();
        
        /// <summary>
        /// Stop calculating Visibility
        /// </summary>
        /// <param name="resetVisibility">Whether or not to re-hide tiles outside of current field of view</param>
        void Disable(bool resetVisibility);
        
        /// <summary>
        /// Adds a tile to the list of currently visible tiles
        /// </summary>
        /// <param name="terrain">The tile we've explored</param>
        void UpdateTerrainSeen(RogueLikeEntity terrain);
        
        /// <summary>
        /// Removes a tile from the list of currently visible tiles and add it to the list of explored tiles
        /// </summary>
        /// <param name="terrain">The tile we've left</param>
        void UpdateTerrainUnseen(RogueLikeEntity terrain);
        
        /// <summary>
        /// Adds another entity (from the spatial map) to the list of currently visible entities
        /// </summary>
        /// <param name="entity">The newly seen entity</param>
        void UpdateEntitySeen(RogueLikeEntity entity);
        
        /// <summary>
        /// Removes an entity (from the spatial map) from the list of currently visible entities
        /// </summary>
        /// <param name="entity">The recently lost entity</param>
        void UpdateEntityUnseen(RogueLikeEntity entity);
    }
}