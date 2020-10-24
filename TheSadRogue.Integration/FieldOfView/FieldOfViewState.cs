namespace TheSadRogue.Integration.FieldOfView
{
    /// <summary>
    /// Possible states for the FOVVisibilityHandler to be in.
    /// </summary>
    public enum FieldOfViewState
    {
        /// <summary>
        /// Enabled state -- FOVVisibilityHandler will actively set things as seen/unseen when appropriate.
        /// </summary>
        Enabled,
        /// <summary>
        /// Disabled state.  All items in the map will be set as seen, and the FOVVisibilityHandler
        /// will not set visibility of any items as FOV changes or as items are added/removed.
        /// </summary>
        DisabledResetVisibility,
        /// <summary>
        /// Disabled state.  No changes to the current visibility of terrain/entities will be made, and the FOVVisibilityHandler
        /// will not set visibility of any items as FOV changes or as items are added/removed.
        /// </summary>
        DisabledNoResetVisibility
    }
}