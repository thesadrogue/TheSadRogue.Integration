# TheSadRogue.Integration

This is an integration library that integrates the components of SadConsole, and GoRogue's `GameFramework` system.  It works by combining similar concepts from both GoRogue and SadConsole, and creating all the necessary functions for components to work with both seamlessly.

The namespaces match the directory structure, except the namespaces omit "The":

```
 TheSadRogue.Integration/
  +- Components/                            # Framework integrating SadConsole and GoRogue components into one component system
  |   +- PlayerControlsComponent                   # Component for basic movement & actions
  |   +- RogueLikeComponentBase                    # Base classes for creating components that work with SadConsole and GoRogue
  |
  +- FieldOfView/                           # Implements system for handling visibility changes to map objects resulting from the player's FOV
  |   +- BasicFieldOfViewHandler                   # Basic implementation of a field of view handler that uses a decorator to tint terrain outside of FOV
  |   +- FieldOfViewHandlerBase                    # Abstract base class for a map component allowing interfacing with FOV and changing visibility of map elements
  |   +- Memory/                            # System of FOV handlers/terrain classes for implementing player "memory"
  |       +- DimmingMemoryFieldOfViewHandler       # Memory handler that handles tiles vanishing into memory by dimming their foreground and background colors
  |       +- ForegroundChangeFieldOfViewHandler    # Memory handler that handles tiles tiles vanishing into memory by changing their foreground color to the specified value
  |       +- MemoryAwareRogueLikeCell              # A subclass of RogueLikeCell to use for terrain when using memory field of view handlers
  |       +- MemoryFieldOfViewHandlerBase          # Base class for implementing a FOV handler that keeps cells as they last appeared when they exit FOV (until they are seen again)
  |
  +- Maps/                                  # Implements maps that are both recognizable by GoRogue and able to be easily rendered by SadConsole
  |   +- AdvancedRogueLikeMap                      # An advanced map that is recognized by GoRogue and can be rendered independently by many different surfaces
  |   +- RogueLikeMap                              # A basic map that is recognized by GoRogue and is also an object positionable and renderable in the SadConsole screen heirarchy
  |   +- RogueLikeMapBase                          # Abstract base class for a map recognizable by GoRogue and able to integrate with SadConsole surfaces.
  |
  +- Rendering/                            # Implements rendering-related functionality
  |   +- MapTerrainCellSurface                     # SadConsole ICellSurface used by map to expose its terrain as a surface
  |   +- TerrainAppearance                         # Appearance object inheriting from ColoredGlyph used for RogueLikeCell.
  |
  +- RogueLikeCell                         # A terrain object recognizable by both SadConsole and GoRogue
  +- RogueLikeEntity                       # An entity recognized by both SadConsole and GoRogue
```

## Usage

In your game, your map class should inherit from `RogueLikeMap`, or `AdvancedRogueLikeMap` if you need to render it independently in multiple places.  Your terrain objects should derive from `RogueLikeCell`, your entities should derive from `RogueLikeEntity`, and your components should inherit from `RogueLikeComponentBase` or `RogueLikeComponentBase<T>`.

Entities and terrain objects are added to the map just like you would with GoRogue's `Map`; they will automatically be rendered appropriately with SadConsole.  A `RogueLikeMap` may be set as the screen to render in SadConsole, or used in the screen hierarchy just like any other SadConsole object.  An `AdvancedRogueLikeMap` lets you create independent renderers that can render different portions of the same map to different areas of the screen.

When you add a component of any variety to an entity, you should add it to the `AllComponents` list; it will be added to both GoRogue's and SadConsole's component collections as applicable.  If you wanted, you could add a component to _only_ SadConsole's collection by adding it to `SadComponents`, but generally it is preferable to let them be managed automatically.


## Examples
A code example that creates a map with a movable player can be found in the `ExampleGame/` folder.
