# TheSadRogue.Integration

This is an integration library that integrates the components of SadConsole, and GoRogue's `GameFramework` system.  It works by combining similar concepts from both GoRogue and SadConsole, and creating all the necessary functions for components to work with both seamlessly.

The namespaces match the directory structure, except the namespaces omit "The":

```
 TheSadRogue.Integration/
  +- Components/                            # Framework integrating SadConsole and GoRogue components into one component system
  |   +- RogueLikeComponentBase                    # Base classes for creating components that work with SadConsole and GoRogue
  |
  +- FieldOfView/                           # Implements system for handling visibility changes to map objects resulting from the player's FOV
  |   +- BasicFieldOfViewHandler                   # Basic implementation of a field of view handler that uses a decorator to tint terrain outside of FOV
  |   +- FieldOfViewHandlerBase                    # Abstract base class for a map component allowing interfacing with FOV and changing visibility of map elements
  |   +- Memory/                            # System of FOV handlers/terrain classes for implementing player "memory"
  |       +- DimmingMemoryFieldOfViewHandler       # MemoryFieldOfViewHandlerBase implementation that handles tiles vanishing into memory by dimming their foreground and background colors
  |       +- ForegroundChangeFieldOfViewHandler    # MemoryFieldOfViewHandlerBase implementation that handles tiles tiles vanishing into memory by changing their foreground color to the specified value
  |       +- MemoryAwareRogueLikeCell              # A subclass of RogueLikeCell to use for terrain when using memory field of view handlers from this namespace
  |       +- MemoryFieldOfViewHandlerBase          # Base class for implementing a FOV handler that keeps cells as they last appeared when they exit FOV (until they are seen again)
  |
  +- Keybindings/                           # Implements a component that can be attached to a RogueLikeEntity to implement keybindings for actions and movements
  |   +- InputKey                                  # Simple class used to represent a key and associated modifiers (shift, ctrl, alt)
  |   +- KeyModifiers                              # Implements an enumeration of supported modifier keys
  |   +- PlayerKeybindingsComponent                # A component that can be attached to a RogueLikeEntity to implement keybindings for actions and movements
  |
  +- Maps/                                  # Implements maps that are both recognizable by GoRogue and able to be easily rendered by SadConsole
  |   +- MapEntityForwarderComponent               # Component that is used internally by RogueLikeMap to forward handlers for SadConsole object items through to entities.
  |   +- MapTerrainCellSurface                     # A custom ICellSurface used internally by RogueLikeMap in order to expose the terrain layer as a cell surface.
  |   +- RogueLikeMap                              # A GoRogue Map that integrates RogueLikeCell and RogueLikeEntity into a structure which SadConsole can easily render, and can be positionable as a screen in the SadConsole object hierarchy.
  |
  +- RogueLikeCell                         # A terrain object recognizable by both SadConsole and GoRogue
  +- RogueLikeEntity                       # An entity recognized by both SadConsole and GoRogue
  +- TerrainAppearance                     # A subclass of ColoredGlyph that is aware of the object whose appearance it represents
```

## Usage Intent/Guidelines
In your game, your map class should be or inherit from a `RogueLikeMap`.  You then need to add the map to the SadConsole screen hierarchy in order to allow it to process events and render correctly.  If you need to render the map independently in multiple places on the screen, you will need to specify `null` for the `defaultRendererParams`, to indicate that there won't be an initial default renderer.  You can then use the map's `CreateRenderer` function to generate renderers.  In this case, you _must_ add _both_ the map itself _and_ any renderers you create to SadConsole's screen object hierarchy, in order to ensure that events are processed correctly.  Any renderers you create should be unhooked from the map by calling the `RogueLikeMap.RemoveRenderer` function once they are no longer being used, in order to prevent resource leaks.

Your terrain objects should derive from `RogueLikeCell`, and your entities (eg. non-terrain objects) should derive from `RogueLikeEntity`.  Entities and terrain objects are added to the map just like you would with GoRogue's `Map`; they will automatically be rendered appropriately with SadConsole.

Any components you intend to attach to `RogueLikeEntity` instances may (but are not required to) inherit from `RogueLikeComponentBase` or `RogueLikeComponentBase<T>`. When you add a component of any variety to an entity or a `RogueLikeMap`, you should add it to the `AllComponents` list; it will be added to both GoRogue's and SadConsole's component collections as applicable.  If you wanted, you could add a component to _only_ SadConsole's collection by adding it to `SadComponents`, but generally it is preferable to let them be managed automatically.

## Examples
A code example that creates a map with a movable player can be found in the `ExampleGame/` folder.  A more complex example can be found [here](https://github.com/Chris3606/SadRogueExample).
