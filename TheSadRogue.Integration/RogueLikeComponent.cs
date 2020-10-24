using System;
using GoRogue.GameFramework;
using SadConsole;
using SadConsole.Input;

namespace TheSadRogue.Integration
{
    public class RogueLikeComponent : SadConsole.Components.IComponent, GoRogue.GameFramework.Components.IGameObjectComponent
    {
        public int SortOrder { get; }
        public bool IsUpdate { get; }
        public bool IsRender { get; }
        public bool IsMouse { get; }
        public bool IsKeyboard { get; }
        public IGameObject? Parent { get; set; }
        
        public void Update(IScreenObject host, TimeSpan delta) //todo
        {
            throw new NotImplementedException();
        }

        public void Render(IScreenObject host, TimeSpan delta) //todo
        {
            throw new NotImplementedException();
        }

        public void ProcessMouse(IScreenObject host, MouseScreenObjectState state, out bool handled) //todo
        {
            throw new NotImplementedException();
        }

        public void ProcessKeyboard(IScreenObject host, Keyboard keyboard, out bool handled) //todo
        {
            throw new NotImplementedException();
        }

        public void OnAdded(IScreenObject host) //todo
        {
            throw new NotImplementedException();
        }

        public void OnRemoved(IScreenObject host) //todo
        {
            throw new NotImplementedException();
        }
    }
}