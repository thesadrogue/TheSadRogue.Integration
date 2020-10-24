using System;
using GoRogue.GameFramework;
using SadConsole;
using SadConsole.Input;

namespace TheSadRogue.Integration
{
    /// <summary>
    /// A Component that works with both SadConsole and GoRogue.
    /// </summary>
    /// <remarks>
    /// The intended use is for you to derive from this class and
    /// then override the methods you need. The base class there-
    /// fore has declared empty implementations so that no undo
    /// processing or errors occur.
    /// </remarks>
    public abstract class RogueLikeComponent : IRogueLikeComponent
    {
        public int SortOrder { get; }
        public bool IsUpdate { get; }
        public bool IsRender { get; }
        public bool IsMouse { get; }
        public bool IsKeyboard { get; }
        public IGameObject? Parent { get; set; }
        public RogueLikeComponent(bool isUpdate, bool isRender, bool isMouse, bool isKeyboard, int sortOrder)
        {
            IsUpdate = isUpdate;
            IsRender = isRender;
            IsMouse = isMouse;
            SortOrder = sortOrder;
            IsKeyboard = isKeyboard;
        }

        public virtual void Update(IScreenObject host, TimeSpan delta)
        {
            
        }

        public virtual void Render(IScreenObject host, TimeSpan delta)
        {
            
        }

        public virtual void OnAdded(IScreenObject host)
        {
            
        }

        public virtual void OnRemoved(IScreenObject host)
        {
            
        }

        public virtual void ProcessMouse(IScreenObject host, MouseScreenObjectState state, out bool handled)
        {
            handled = false;
        }

        public virtual void ProcessKeyboard(IScreenObject host, Keyboard keyboard, out bool handled)
        {
            handled = false;
        }
    }
}