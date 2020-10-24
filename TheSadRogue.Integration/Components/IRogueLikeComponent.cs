using System;
using GoRogue.GameFramework;
using SadConsole;
using SadConsole.Input;

namespace TheSadRogue.Integration
{
    public interface IRogueLikeComponent : SadConsole.Components.IComponent, GoRogue.GameFramework.Components.IGameObjectComponent
    {
    }
}