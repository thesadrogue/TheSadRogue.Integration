namespace TheSadRogue.Integration.Components
{
    /// <summary>
    /// An interface for a component that works with both SadConsole and GoRogue.
    /// </summary>
    /// <remarks>
    /// This Interface is a shorthand for both SadConsole.Components.IComponent and GoRogue.GameFramework.Components.IGameObjectComponent
    /// </remarks>
    public interface IRogueLikeComponent : SadConsole.Components.IComponent, GoRogue.GameFramework.Components.IGameObjectComponent
    {
    }
}