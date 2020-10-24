# TheSadRogue.Integration

An integration library between SadConsole & GoRogue. 

This library will contain classes to help you make a game with Entities and Components that work perfectly with both systems. It is specifically intended to get a quicker start for people who are using the exist `SadConsole.GoRogueHelpers` library, and anyone who wants to work the with new versions of both libraries (SadConsole version 9 and GoRogue version 3).


## Design Philosophy

An Integration library should help people create games using SadConsole & GoRogue by creating classes that combine the similar concepts present in both libraries.

If both libraries share a class that is similar in concept, then that class should have an equivalent `RogueLike-` class in `TheSadRogue.Integration` that implements both in a way that is simple, quick, and elegant.

One of the things that irked me when i was first starting out is that when I followed code along, i often went 6+ layers deep in inheritance. It is much easier for me to remember and conceptualize short inheritance chains when I program, to prevent from there being too many moving pieces to keep in mind at once. 

For that reason, I'd like to implement the similar interfaces directly, and inherit from __NO__ class whenever possible. This should keep the inheritance chains short; `RogueLikeEntity` implements `SadConsole.Entity`, `IConsole`, & `IGameObject`; `RogueLikeComponent` will impement `IGameObjectComponent` and `IConsoleComponent` but NOT `Component`, etc.

the integration library should be _very_ welcoming to newcomers. It should contain the features that currently exist to get people developing faster, such as the dotnet template, starting example, & nuget package. It should also include a more in-depth tutorial to get people started using gorogue, sadconsole, and the integration library.

## Current Progress

### RogueLikeMap

- Currently bare-bones with no SC functionality.

### RogueLikeEntity

- Currently generating correctly, added to the map correctly, and being translated into ColoredGlyph correctly.
- No component-related methods have been implemented yet.

### RogueLikeComponent

- Currently not implemented. Coming soon!

## Example Game

The example game has no content. It exists only to serve as a proof-of-concept and an example to those starting out.

### ExampleGameUi

This class is the UI of the game; it contains a map section, and a section for message logs. Coming soon:

- a player entity which moves
- field of view
- other creatures that path
- doors that open, close, and lock
- items that can contain other items
- items that can be picked up
- some semblence of health
- some semblence of combat