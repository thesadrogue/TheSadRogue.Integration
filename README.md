# TheSadRogue.Integration
An integration library between [SadConsole](https://github.com/Thraka/SadConsole) and [GoRogue](https://github.com/Chris3606/GoRogue), and the spiritual successor to the [previous integration library](https://github.com/thesadrogue/SadConsole.GoRogueHelpers).

## Why a whole new library?
Both SadConsole and GoRogue are undergoing extensive re-writes for their new versions (SadConsole version 9 and GoRogue version 3).  This includes a great many breaking changes to the APIs.  One of the most notable changes is the inclusion of [TheSadRogue.Primitives](https://github.com/thesadrogue/TheSadRogue.Primitives), a library of commonly-used data types when working with a discreet, 2D grid, as a dependency for both libraries.  Because of this change, projects that use both SadConsole(v8) and GoRogue(v2) cannot upgrade one independently of the other without tons of conflicting type declarations, and upgrading everything else is non-trivial as well. 

For the purpose of maintaining readable code that is (relatively) hack-free and friendly to beginners, it is more elegant to start a new integration library from scratch.  This also provides an opportunity to fix confusing namespace structure and other problems with the original integration library.

## What's in the integration library?
This library contains classes that bridge functionality between SadConsole and GoRogue. It is specifically intended to implement similar interfaces from both libraries, and therefore give novice developers a quick start to developing a rogue-like (or lite) game. 

The library also provides:
- A `dotnet new` template for easily creating new projects (TODO)
- A companion tutorial (TODO)
- An example RogueLike game to get you started (WIP)
- Unit tests for its functionality

### The Integration Library
The integration library core functionality is located in `TheSadRogue.Integration/`.  Details about its functionality an usage can be found in [its readme](TheSadRogue.Integration/README.md).

### A Project Template (TODO)
This project will also provide a template compatible with `dotnet new`, that will be distributed via NuGet.  This is not yet complete, but will make creating new projects with boilerplate code in place quick and easy.

### A Companion Tutorial (TODO)
Eventually, a roguelike tutorial will be written using the integration library that will demonstrate the features, however it does not yet exist.

### An Example Game (WIP)
An example game is also provided that demonstrates the usage of many of the features of the integration library in the `ExampleGame/` folder. Details about the example may be found in [its readme](ExampleGame/README.md).  The example is still a work in progress.

### Unit Tests
Unit tests for integration library functionality are included in `TheSadRogue.Integration.Tests/`.  The tests may be provide useful example code, and also show how to create mocks of various SadConsole structures that enable unit testing of SadConsole projects.


## How do I get started?
As detailed above, a template will be provided to create new projects.  Instructions on its use will be placed here after it is created.

Additionally, you may find the `ExampleGame/` helpful as it provides well-commented example code that utilizes integration library features.


## Current Progress
This library is still in alpha and under active development.  Here is a rough summary of the current state of each part.

### Integration Library
- Map and map object structures completed
    - Allows rendering map directly as a SadConsole object, as well as rendering the same map via multiple renderers
    - Supports viewports and other concepts implemented directly in SadConsole
- Structure for controlling visibility based on FOV in place
    - Extension that implements the concept of character "memory" also in place, which shows how the base classes can be utilized
- Component integration framework in-place
    - Components may be added to both maps and map objects
    - Supports a single base class and single component list per object that combines the GoRogue and SadConsole concept of components into one
	- Includes a convenient component for implementing player controls
- NuGet package creation implemented (no actual NuGet release yet)
- **Missing** the following features from the previous integration library
    - Everything in the `Tiles` namespace
	    - May or may not be re-implemented here
		- Library architecture/structures have changed such that it may no longer be useful as previously implemented; TBD
	- `Action` namespace/system and associated components
	    - Needs to be re-evaluated since GoRogue and SadConsole components have been combined
		- May have some overlap with GoRogue's effect system
	- World generation code from `Maps.Generators` namespace
	    - Potentially possible to integrate into GoRogue's new map generation framework
		- Wrappers around map class need to be re-evaluated due to architecture changes
	- "Game-frame"/turn system
	    - Implementation may be greatly simplified since GoRogue/SadConsole components have been combined
		- May be possible to integrate a more diverse feature set

### New Project Template
- Not yet implemented

### Companion Tutorial
- Not yet implemented

### Example Game
- Generates map using GoRogue's map generation system
- Displays map via viewport that centers on the player's position
- Implements controllable player
- Implements field-of-view and character memory
- **Not Yet Implemented**
    - Enemies/NPCs
	- Item/loot placement and inventory
	- Working, lockable doors
	- Any semblance of health/combat
	- Goals/win conditions