# TheSadRogue.Integration

This is an integration library that integrates the components of SadConsole, and GoRogue's `GameFramework` system. It works by combining similar concepts from both GoRogue and SadConsole, and creating all the necessary functions for components to work with both seamlessly.

The namespaces match the directory structure, except the namespaces omit "The":

```
 TheSadRogue.Integration/
  +- Components/
  |   +- PlayerControlsComponent          # Component for basic movement & actions
  |   +- RogueLikeComponentBase           # Base classes for creating components that work with SadConsole and GoRogue
  +- FieldOfView/ (NOT YET IMPLEMENTED)
  |   +- FieldOfViewHandlerBase           # Abstract base class for interfacing with FOV and changing visibility of map elements
  |   +- FieldOfViewState                 # Enum whose values represent the state of a FieldOfViewHandler
  +- Maps/
  |   +- AdvancedRogueLikeMap             # An advanced map that is recognized by GoRogue and can be rendered independently by many different surfaces
  |   +- RogueLikeMap                     # A basic map that is recognized by GoRogue and is also an object positionable and renderable in the SadConsole screen heirarchy
  |   +- RogueLikeMapBase                 # Abstract base class for a map recognizable by GoRogue and able to integrate with SadConsole surfaces.
  +- RogueLikeCell                        # A terrain object recognizable by both SadConsole and GoRogue
  +- RogueLikeEntity                      # An entity recognized by SadConsole & GoRogue
```

## Usage

In your game, your map class should inherit from `RogueLikeMap`, or `AdvancedRogueLikeMap` if you need to render it independently in multiple places.  Your terrain objects should derive from `RogueLikeCell`, your entities should derive from `RogueLikeEntity`, and your components should inherit from `RogueLikeComponentBase` or `RogueLikeComponentBase<T>`.

Entities and terrain objects are added to the map just like you would with GoRogue's `Map`; they will automatically be rendered appropriately with SadConsole.  A `RogueLikeMap` may be set as the screen to render in SadConsole, or used in the screen hierarchy just like any other SadConsole object.  An `AdvancedRogueLikeMap` lets you create independent renderers that can render different portions of the same map to different areas of the screen.

When you add a component of any variety to an entity, you should add it to the `AllComponents` list; it will be added to both GoRogue's and SadConsole's component collections as applicable.  If you wanted, you could add a component to _only_ SadConsole's collection by adding it to `SadComponents`, but generally it is preferable to let them be managed automatically.


## Examples
A code example that creates a map with a movable player can be found in the `ExampleGame/` folder.
