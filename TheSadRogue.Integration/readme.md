# TheSadRogue.Integration

This is the integration library. It works by combining similar concepts from both GoRogue and SadConsole, and creating all the necessary functions to muck with both seemlessly.

The Namespaces match the directory structure:

```
 TheSadRogue.Integration/ 
  +- Components/
  |   +- IRogueLikeComponent.cs              # base interface for components
  |   +- PlayerControlsComponent.cs          # basic movement & actions 
  |   +- RogueLikeComponent.cs               # an abstract type that takes care of things for you
  +- Extensions/
  |   +- ICellSurfaceExtensions.cs
  |   +- IMapViewExtensions.cs 
  |   +- IReadOnlySpatialMapExtensions.cs
  +- FieldOfView/
  |   +- IFieldOfViewHandler.cs              # interface for FOV components
  |   +- FieldOfViewHandler.cs               # an implementation of said FOV
  |   +- FieldOfViewState.cs                 # an enum to help with calculating FOV
  +- RogueLikeEntity.cs                      # An entity recognized by SadConsole & GoRogue
  +- RogueLikeMap.cs                         # A Map that contains entites and can be rendered to a screen surgace   
```

## Usage

In your game, your `Map` class should inherit from `RogueLikeMap`, your entities should derive from `RogueLikeEntity`, and your components should inherit from `RogueLikeComponent`.

When you create a new entity, you will get back a class that contains all the necessary functions to manage and use components.

When you add a `RogueLikeComponent` to such an entity via the `AddComponent` method, that component is added to both Sadconsole's and GoRogue's component collection.

If you wanted, you could add a `RogueLikeComponent` to __only__ SadConsole's collection by invoking `AddSadComponent`, or __only__ GoRogue's collection by invoking `AddGoRogueComponent`.

If you invoke `RogueLikeMap.SetTerrain` to add an entity as a piece of terrain then that entity will display when the `Map` is added to a screen surface.

The following example code will create a simple maze (known bug - player doesn't appear despite being created):

```
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using SadConsole;
using SadRogue.Primitives;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Extensions;

namespace ExampleGame
{
    class Program
    {
        public const int Width = 80;
        public const int Height = 25;
        private const int _mapWidth = 80;
        private const int _mapHeight = 25;

        static void Main(string[] args)
        {
            SadConsole.Game.Create(Width, Height);
            SadConsole.Game.Instance.OnStart = Init;
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        private static void Init()
        {
            var map = new RogueLikeMap(_mapWidth, _mapHeight, 4, Distance.Manhattan);
            GenerateMap(map);
            
            var player = new RogueLikeEntity( (16,16), 1, layer: 1);
            player.AddComponent(new PlayerControlsComponent());
            map.AddEntity(player);

            var screen = new ScreenSurface(_mapWidth, _mapHeight, map.TerrainSurface.ToArray());
            SadConsole.GameHost.Instance.Screen.Children.Add(screen);
        }
        
        static void GenerateMap(RogueLikeMap map)
        {
            var generator = new Generator(map.Width, map.Height);
            generator.AddSteps(DefaultAlgorithms.DungeonMazeMapSteps());
            generator = generator.Generate();
		      
            var underlyingMap = generator.Context.GetFirst<ISettableMapView<bool>>();
            for (int i = 0; i < underlyingMap.Width; i++)
            {
                for (int j = 0; j < underlyingMap.Height; j++)
                {
                    Point here = (i, j);
                    bool walkable = underlyingMap[i, j];
                    map.SetTerrain(new RogueLikeEntity(here, walkable ? '.' : '#', walkable, walkable));
                }
            }
        }
    }
}
```
 