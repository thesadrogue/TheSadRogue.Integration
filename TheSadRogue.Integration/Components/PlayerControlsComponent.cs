using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GoRogue;
using GoRogue.GameFramework;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;

namespace TheSadRogue.Integration.Components
{
    /// <summary>
    /// A basic component that moves a character based on keyboard input
    /// </summary>
    public class PlayerControlsComponent : RogueLikeComponentBase
    {
        /// <summary>
        /// A mapping of Keys to an action that can be performed
        /// </summary>
        public ReadOnlyDictionary<Keys, Action> Actions => _actions.AsReadOnly();
        private readonly Dictionary<Keys, Action> _actions;
        /// <summary>
        /// A mapping of Keys to an action that can be performed
        /// </summary>
        public ReadOnlyDictionary<Keys, Direction> Motions => _motions.AsReadOnly();
        private readonly Dictionary<Keys, Direction> _motions;
        
        /// <summary>
        /// Creates a new component that controls it's parent entity via keystroke
        /// </summary>
        public PlayerControlsComponent() : base(false, false, false, true, 1)
        {
            _actions = new Dictionary<Keys, Action>();
            _motions = new Dictionary<Keys, Direction>()
            {
                { Keys.Left, Direction.Left},
                { Keys.Right, Direction.Right},
                { Keys.Up, Direction.Up},
                { Keys.Down, Direction.Down},
            };
        }

        /// <summary>
        /// Create a PlayerControlsComponent with the provided dictionary of keys to actions
        /// </summary>
        /// <param name="actions">A dictionary of Keys to Actions</param>
        public PlayerControlsComponent(Dictionary<Keys, Action> actions) : this()
        {
            _actions = actions;
        }
        /// <summary>
        /// Create a PlayerControlsComponent with the provided dictionary of keys to movement directions
        /// </summary>
        /// <param name="motions">A dictionary of Keys to Movement Directions</param>
        public PlayerControlsComponent(Dictionary<Keys, Direction> motions) : this()
        {
            _motions = motions;
        }    
        /// <summary>
        /// Create a PlayerControlsComponent with the provided dictionary of keys to movement directions and keys to actions
        /// </summary>
        /// <param name="motions"></param>
        /// <param name="actions"></param>
        public PlayerControlsComponent(Dictionary<Keys, Direction> motions, Dictionary<Keys, Action> actions) : this()
        {
            _motions = motions;
            _actions = actions;
        }

        /// <summary>
        /// Adds a new action to the keystrokes being listened for
        /// </summary>
        /// <param name="key">The Key to listen for</param>
        /// <param name="action">The action to perform when said key is pressed</param>
        public void AddKeyCommand(Keys key, Action action)
        {
            if (!_actions.ContainsKey(key))
                _actions.Add(key, action);
        }

        /// <summary>
        /// Removes a key command from the set we're listening for
        /// </summary>
        /// <param name="key">The key to remove</param>
        public void RemoveKeyCommand(Keys key)
        {
            if (_actions.ContainsKey(key))
                _actions.Remove(key);
        }
        
        /// <inheritdoc />
        public override void ProcessKeyboard(IScreenObject host, Keyboard keyboard, out bool handled)
        {
            foreach (var motion in Motions)
                if (keyboard.IsKeyPressed(motion.Key))
                    if(Parent!.CanMoveIn(motion.Value))
                        Parent!.Position += motion.Value;

            foreach (KeyValuePair<Keys,Action> action in Actions)
                if (keyboard.IsKeyPressed(action.Key))
                    action.Value();
            
            handled = true;
        }
    }
}