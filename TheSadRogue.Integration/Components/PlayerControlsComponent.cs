using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GoRogue;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;

namespace TheSadRogue.Integration
{
    public class PlayerControlsComponent : RogueLikeComponent
    {
        private readonly Dictionary<Keys, Action> _actions;
        public ReadOnlyDictionary<Keys, Action> Actions => _actions.AsReadOnly();
        
        public PlayerControlsComponent() : base(false, false, false, true, 1)
        {
            _actions = new Dictionary<Keys,Action>();
        }

        public PlayerControlsComponent(Dictionary<Keys, Action> actions) : this()
        {
            _actions = actions;
        }

        public void AddKeyCommand(Keys key, Action action)
        {
            if (!_actions.ContainsKey(key))
                _actions.Add(key, action);
        }

        public void RemoveKeyCommand(Keys key, Action action)
        {
            if (_actions.ContainsKey(key))
                _actions.Remove(key);
        }
        
        public override void ProcessKeyboard(IScreenObject host, Keyboard keyboard, out bool handled)
        {
            if (keyboard.IsKeyPressed(Keys.Left))
                Parent.Position += Direction.Left;
            
            if (keyboard.IsKeyPressed(Keys.Right))
                Parent.Position += Direction.Right;
            
            if (keyboard.IsKeyPressed(Keys.Up))
                Parent.Position += Direction.Up;
            
            if (keyboard.IsKeyPressed(Keys.Down))
                Parent.Position += Direction.Down;

            foreach (KeyValuePair<Keys,Action> action in Actions)
                if (keyboard.IsKeyPressed(action.Key))
                    action.Value();
            
            handled = true;
        }
    }
}