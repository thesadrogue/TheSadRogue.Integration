# TheSadRogue.Integration

An integration library between [SadConsole](https://github.com/Thraka/SadConsole ) & [GoRogue](https://github.com/Chris3606/GoRogue ), and the spiritual successor to the [previous integration library](https://github.com/thesadrogue/SadConsole.GoRogueHelpers )

### Why a whole new library?

Both SadConsole & GoRogue are undergoing extensive re-writes for their new versions (SadConsole version 9 and GoRogue version 3). This includes a great many breaking changes to the APIs, the most extreme of which is the inclusion of [TheSadRogue.Primitives](https://github.com/thesadrogue/TheSadRogue.Primitives ), a library of commonly-used data types when working with a discreet, 2D grid. Because of this change, projects that use both SadConsole(v8) and GoRogue(v2) cannot upgrade one independently of the other without tons of conflicting type declarations, and upgrading everything else is non-trivial as well. 

For the purpose of maintaining readable code that is (relatively) hack-free and friendly to beginners, it would be more elegant to start a new integration library from scratch. This has the side effect of fixing the confusing namespace structure (or lack thereof) of the original integration library.

### What's in the integration library?

This library contains classes that bridge functionality between SadConsole and GoRogue. It is specifically intended to implement similar interfaces from both libraries, and therefore give novice developers a quick start to developing a rogue-like (or lite) game. 

To further that last bit, this library comes complete with:
- An example RogueLike game to get you started (WIP)
- A companion tutorial (TODO)
- A `dotnet new` template (TODO)

The Namespaces match the directory structure:

```
Solution/
 +- ExampleGame/ 
 |   +- Program.cs                              # creates & starts the game
 |   +- GameUi.cs                               # manages the screen space 
 |   +- MapGeneration/
 |   |   +- MapGenerator.cs                     # coordinates GenerationSteps
 |   |   +- GenerationSteps/
 |   |       +- BackroomsGenerationStep.cs      # produces a cluster of rectangular rooms
 |   |       +- CaveGenerationStep.cs           # a small cellular automata 
 |   |       +- CaveSeedingStep.cs              # initialize the map for the cave generation to work
 |   |       +- CryptGenerationStep.cs          # create a brick pattern of rooms with thick walls 
 |   |       +- SpiralGenerationStep.cs         # carves a spiral out of a solid area
 |      
 +- TheSadRogue.Integration/ 
 |   +- Components/
 |   |   +- IRogueLikeComponent.cs              # base interface for components
 |   |   +- PlayerControlsComponent.cs          # basic movement & actions 
 |   |   +- RogueLikeComponent.cs               # an abstract type that takes care of things for you
 |   +- Extensions/
 |   |   +- ICellSurfaceExtensions.cs
 |   |   +- IMapViewExtensions.cs 
 |   |   +- IReadOnlySpatialMapExtensions.cs
 |   +- FieldOfView/
 |   |   +- IFieldOfViewHandler.cs              # interface for FOV components
 |   |   +- FieldOfViewHandler.cs               # an implementation of said FOV
 |   |   +- FieldOfViewState.cs                 # an enum to help with calculating FOV
 |   +- RogueLikeEntity.cs                      # An entity recognized by SadConsole & GoRogue
 |   +- RogueLikeMap.cs                         # A Map that contains entites and can be rendered to a screen surgace   
 | 
 +- Tests/                                      # Might be useful to look at if you're stuck
     +- ...
```

### How do I get started?

Here is an example `Program.cs` that will generate and display a map:
```
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using SadConsole;
using SadRogue.Primitives;
using TheSadRogue.Integration;
using TheSadRogue.Integration.Components;
using TheSadRogue.Integration.Extensions;

namespace ExampleGame
{
    class Program
    {
        public const int Width = 80;
        public const int Height = 25;
        private const int MapWidth = 80;
        private const int MapHeight = 25;

        static void Main(string[] args)
        {
            Game.Create(Width, Height);
            Game.Instance.OnStart = Init;
            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        private static void Init()
        {
            var map = new RogueLikeMap(MapWidth, MapHeight, 4, Distance.Manhattan);
            GenerateMap(map);
            
            var player = new RogueLikeEntity( (16,16), 1, layer: 1);
            player.AddComponent(new PlayerControlsComponent());
            map.AddEntity(player);

            var screen = new ScreenSurface(MapWidth, MapHeight, map.TerrainSurface.ToArray());
            GameHost.Instance.Screen.Children.Add(screen);
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

## Current Progress

### RogueLikeMap

- Generates
- Can be added to a `ScreenSurface`
- Gets a `ColoredGlyph` from the terrain view
- Doesn't move viewport around yet
- doesn't update entities when they move

### RogueLikeEntity

- Is likely to refactor when SadConsole's `EntityLite` is completed
- No component-related methods have been implemented yet.

### RogueLikeComponent

- Completed interface and abstract class
- Player Controls Component adds movement control to an entity

## Example Game

- Generates terrain according to a composite of different algorithms
- Generates a player character and gives it a `PlayerControlsComponent`
- Adds the map to the screen and displays it
- Reacts to player control
- Doesn't manage field of view
- Doesn't have other creatures
- Doesn't guarantee that the entire map can be accessed 
- Doesn't place doors that open, close, and lock
- Doesn't place items
- Doesn't contain any semblance of health or combat
- Doesn't have any goals to pursue