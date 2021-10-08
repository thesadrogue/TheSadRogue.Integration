using SadRogue.Integration;
using SadRogue.Integration.FieldOfView.Memory;
using SadRogue.Integration.Keybindings;
using SadRogue.Primitives;

namespace TheSadRogue.Integration.Templates.MonoGame
{
    /// <summary>
    /// Simple class with some static functions for creating map objects.  You can choose to use inheritance to create
    /// your objects instead, but the integration library offers a robust component system that integrates both
    /// SadConsole's and GoRogue's components into one interface, so you're not bound to do so.  If you _do_ choose to
    /// use composition and have factory functions like this, you may find it helpful to implement the factory system
    /// implemented in the GoRogue.Factories namespace.
    /// </summary>
    internal static class MapObjectFactory
    {
        public static MemoryAwareRogueLikeCell Floor(Point position)
            => new(position, Color.White, Color.Black, '.', (int)MyGameMap.Layer.Terrain);

        public static MemoryAwareRogueLikeCell Wall(Point position)
            => new(position, Color.White, Color.Black, '#', (int)MyGameMap.Layer.Terrain, false);

        public static RogueLikeEntity Player()
        {
            // Create entity with appropriate attributes
            var player = new RogueLikeEntity('@', false, layer: (int)MyGameMap.Layer.Monsters);

            // Add component for controlling player movement via keyboard.  Other (non-movement) keybindings can be
            // added as well
            var motionControl = new CustomPlayerKeybindingsComponent();
            motionControl.SetMotions(PlayerKeybindingsComponent.ArrowMotions);
            motionControl.SetMotions(PlayerKeybindingsComponent.NumPadAllMotions);
            player.AllComponents.Add(motionControl);

            // Add component for updating map's player FOV as they move
            player.AllComponents.Add(new PlayerFOVController());

            return player;
        }

        public static RogueLikeEntity Enemy()
        {
            var enemy = new RogueLikeEntity(Color.Red, 'g', false, layer: (int)MyGameMap.Layer.Monsters);

            // Add AI component to path toward player when in view
            enemy.AllComponents.Add(new EnemyAI());

            return enemy;
        }

    }
}
