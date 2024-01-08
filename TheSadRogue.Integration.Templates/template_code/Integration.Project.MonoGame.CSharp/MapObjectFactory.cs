using SadRogue.Integration;
using SadRogue.Integration.FieldOfView.Memory;
using SadRogue.Integration.Keybindings;

namespace TheSadRogue.Integration.Templates.MonoGame
{
    /// <summary>
    /// Simple class with some static functions for creating map objects.
    /// </summary>
    /// <remarks>
    /// CUSTOMIZATION:  This demonstrates how to create objects based on "composition," which means using components.
    /// The integration library offers a robust component system that integrates both SadConsole's and GoRogue's components
    /// into one interface. You can either add more functions to create more objects, or remove this and
    /// implement the "factory" system in the GoRogue.Factories namespace, which provides a more robust interface for it.
    ///
    /// Note that SadConsole components cannot be attached directly to `RogueLikeCell` or `MemoryAwareRogueLikeCell`
    /// instances for reasons pertaining to performance.
    ///
    /// Alternatively, you can remove this system and choose to use inheritance to create your objects instead - the
    /// integration library also supports creating subclasses or RogueLikeCell and RogueLikeEntity.
    /// </remarks>
    internal static class MapObjectFactory
    {
        public static MemoryAwareRogueLikeCell Floor(Point position)
            => new(position, Color.White, Color.Black, '.', (int)MyGameMap.Layer.Terrain);

        public static MemoryAwareRogueLikeCell Wall(Point position)
            => new(position, Color.White, Color.Black, '#', (int)MyGameMap.Layer.Terrain, false, false);

        public static RogueLikeEntity Player()
        {
            // Create entity with appropriate attributes
            var player = new RogueLikeEntity('@', false, layer: (int)MyGameMap.Layer.Monsters);

            // Add component for controlling player movement via keyboard. Other (non-movement) keybindings can be
            // added as well
            var motionControl = new CustomKeybindingsComponent();
            motionControl.SetMotions(KeybindingsComponent.ArrowMotions);
            motionControl.SetMotions(KeybindingsComponent.NumPadAllMotions);
            player.AllComponents.Add(motionControl);

            // Add component for updating map's player FOV as they move
            player.AllComponents.Add(new PlayerFOVController());

            return player;
        }

        public static RogueLikeEntity Enemy()
        {
            var enemy = new RogueLikeEntity(Color.Red, 'g', false, layer: (int)MyGameMap.Layer.Monsters);

            // Add AI component to path toward player when in view
            enemy.AllComponents.Add(new DemoEnemyAI());

            return enemy;
        }

    }
}
