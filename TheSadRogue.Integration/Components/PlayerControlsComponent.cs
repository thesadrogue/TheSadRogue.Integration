using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using GoRogue;
using GoRogue.GameFramework;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;

namespace SadRogue.Integration.Components
{
    /// <summary>
    /// Modifiers for key commands.
    /// </summary>
    public enum KeyModifiers
    {
        /// <summary>
        /// Represents no key modifiers.
        /// </summary>
        None,
        /// <summary>
        /// Represents the left shift key.
        /// </summary>
        LeftShift,
        /// <summary>
        /// Represents the right shift key.
        /// </summary>
        RightShift,
        /// <summary>
        /// Represents either the left or right shift key.
        /// </summary>
        Shift,
        /// <summary>
        /// Represents the left control key.
        /// </summary>
        LeftCtrl,
        /// <summary>
        /// Represents the right control key.
        /// </summary>
        RightCtrl,
        /// <summary>
        /// Represents either the left or right ctrl key.
        /// </summary>
        Ctrl,
        /// <summary>
        /// Represents the left alt key.
        /// </summary>
        LeftAlt,
        /// <summary>
        /// Represents the right alt key.
        /// </summary>
        RightAlt,
        /// <summary>
        /// Represents either the left or right alt key.
        /// </summary>
        Alt
    };

    /// <summary>
    /// Represents an input state that can be bound to an action.
    /// </summary>
    public readonly struct ControlMapping : IEquatable<ControlMapping>, IMatchable<ControlMapping>
    {
        /// <summary>
        /// Key for the input state.
        /// </summary>
        public readonly Keys Key;

        /// <summary>
        /// Any modifiers related to shift (left-shift, right-shift, or either) that are associated with this control.
        /// </summary>
        public readonly KeyModifiers Shift;

        /// <summary>
        /// Any modifiers related to control (left-ctrl, right-ctrl, or either) that are associated with this control.
        /// </summary>
        public readonly KeyModifiers Ctrl;

        /// <summary>
        /// Any modifiers related to alt (left-alt, right-alt, or either) that are associated with this control.
        /// </summary>
        public readonly KeyModifiers Alt;


        /// <summary>
        /// Creates a new input state.
        /// </summary>
        /// <param name="key">Key for the input state.</param>
        /// <param name="modifiers">Any required modifiers for the input state (SHIFT, CTRL, etc.)</param>
        public ControlMapping(Keys key, params KeyModifiers[] modifiers)
        {
            Key = key;

            Shift = KeyModifiers.None;
            Ctrl = KeyModifiers.None;
            Alt = KeyModifiers.None;
            foreach (var modifier in modifiers)
            {
                switch (modifier)
                {
                    case KeyModifiers.LeftAlt:
                    case KeyModifiers.RightAlt:
                    case KeyModifiers.Alt:
                        if (Alt != KeyModifiers.None)
                            throw new ArgumentException("Only one Alt modifier is allowed.", nameof(modifiers));
                        Alt = modifier;
                        break;
                    case KeyModifiers.LeftCtrl:
                    case KeyModifiers.RightCtrl:
                    case KeyModifiers.Ctrl:
                        if (Ctrl != KeyModifiers.None)
                            throw new ArgumentException("Only one Ctrl modifier is allowed.", nameof(modifiers));
                        Ctrl = modifier;
                        break;
                    case KeyModifiers.RightShift:
                    case KeyModifiers.LeftShift:
                    case KeyModifiers.Shift:
                        if (Shift != KeyModifiers.None)
                            throw new ArgumentException("Only one Shift modifier is allowed.", nameof(modifiers));
                        Shift = modifier;
                        break;
                }
            }
        }

        /// <summary>
        /// Implicitly converts a Keys enumeration value to an input state (with no modifiers), for convenience.
        /// </summary>
        /// <param name="key">Key to convert.</param>
        /// <returns>An input state representing the key given with no modifiers.</returns>
        public static implicit operator ControlMapping(Keys key) => new ControlMapping(key);

        /// <summary>
        /// True if the given keyboard state meets the modifier conditions for this input state.
        /// </summary>
        /// <remarks>
        /// This function ensures that precisely the modifiers the input state specifies are true, and NO others.
        /// </remarks>
        /// <param name="keyboard">Keyboard state to check.</param>
        /// <returns>True if the keyboard state meets the modifier conditions for this input state; false otherwise.</returns>
        public bool ModiferConditionsMet(Keyboard keyboard)
        {
            // Check ctrl state
            if (!MatchesState(keyboard, Ctrl, Keys.LeftControl, Keys.RightControl, KeyModifiers.LeftCtrl,
                KeyModifiers.RightCtrl))
                return false;

            // Check alt state
            if (!MatchesState(keyboard, Alt, Keys.LeftAlt, Keys.RightAlt, KeyModifiers.LeftAlt,
                KeyModifiers.RightAlt))
                return false;

            // Check shift state
            return MatchesState(keyboard, Shift, Keys.LeftShift, Keys.RightShift, KeyModifiers.LeftShift,
                KeyModifiers.RightShift);
        }

        /// <summary>
        /// True if the input state given precisely matches the current one in key and modifiers; false otherwise.
        /// </summary>
        /// <param name="other"/>
        /// <returns/>
        public bool Equals(ControlMapping other) => Matches(other);

        /// <summary>
        /// True if the given object is an input state that precisely matches the current one in key and modifiers;
        /// false otherwise.
        /// </summary>
        /// <param name="obj"/>
        /// <returns/>
        public override bool Equals(object? obj) => obj is ControlMapping m && Matches(m);

        /// <summary>
        /// True if the input state given precisely matches the current one in key and modifiers; false otherwise.
        /// </summary>
        /// <param name="other"/>
        /// <returns/>
        public bool Matches(ControlMapping other)
            => Key == other.Key && Ctrl == other.Ctrl && Alt == other.Alt && Shift == other.Shift;

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(Key, Ctrl, Alt, Shift);

        /// <summary>
        /// True if the two input states specified precisely match in key and modifiers; false otherwise.
        /// </summary>
        /// <param name="lhs"/>
        /// <param name="rhs"/>
        /// <returns/>
        public static bool operator==(ControlMapping lhs, ControlMapping rhs) => lhs.Matches(rhs);

        /// <summary>
        /// True if the two input states differ at all in key or modifiers; false if they are equivalent.
        /// </summary>
        /// <param name="lhs"/>
        /// <param name="rhs"/>
        /// <returns/>
        public static bool operator!=(ControlMapping lhs, ControlMapping rhs) => !lhs.Matches(rhs);

        private static bool MatchesState(Keyboard keyboard, KeyModifiers state, Keys keyLeft, Keys keyRight, KeyModifiers left,
                                  KeyModifiers right)
        {
            // Check key state
            bool leftDown = keyboard.IsKeyDown(keyLeft);
            bool rightDown = keyboard.IsKeyDown(keyRight);

            if (state == KeyModifiers.None) return !(leftDown || rightDown);
            if (state == left) return leftDown && !rightDown;
            if (state == right) return rightDown && !leftDown;

            // Invariants enforced by class allow us to assume this means "either left or right".
            return leftDown && !rightDown || rightDown && !leftDown;
        }
    }

    /// <summary>
    /// A basic component that moves a character based on keyboard input
    /// </summary>
    public class PlayerControlsComponent : RogueLikeComponentBase
    {

        private readonly Dictionary<Keys, List<(ControlMapping control, Action action)>> _actions;
        /// <summary>
        /// A mapping of controls to an action to be performed.
        /// </summary>
        public ReadOnlyDictionary<Keys, List<(ControlMapping control, Action action)>> Actions => _actions.AsReadOnly();

        private readonly Dictionary<Keys, List<(ControlMapping control, Direction direction)>> _motions;
        /// <summary>
        /// A mapping of controls to a direction to generate, which will be passed to the <see cref="MotionHandler"/>.
        /// </summary>
        public ReadOnlyDictionary<Keys, List<(ControlMapping control, Direction direction)>> Motions => _motions.AsReadOnly();


        private Action<Direction> _motionHandler;

        /// <summary>
        /// Function to call in order to handle motions generated from bindings set up in <see cref="Motions"/>.
        /// </summary>
        [AllowNull]
        public Action<Direction> MotionHandler
        {
            get => _motionHandler;
            set => _motionHandler = value ?? DefaultMotionHandler;
        }

        /// <summary>
        /// Creates a new component that controls it's parent entity via keystroke.
        /// </summary>
        /// <param name="motionHandler">
        /// The handler to use to handle any key bindings in <see cref="Motions"/>.  If set to null, a default handler
        /// will be used that simply sets the parent object's Position if possible.
        /// </param>
        public PlayerControlsComponent(Action<Direction>? motionHandler = null)
            : base(false, false, false, true, 1)
        {
            // Initialize dictionaries
            _actions = new Dictionary<Keys, List<(ControlMapping, Action)>>();
            _motions = new Dictionary<Keys, List<(ControlMapping control, Direction direction)>>();

            // Add default motion controls
            AddMotion(Keys.Up, Direction.Up);
            AddMotion(Keys.Right, Direction.Right);
            AddMotion(Keys.Down, Direction.Down);
            AddMotion(Keys.Left, Direction.Left);

            // Add motion handler, or default if one was not specified
            _motionHandler = motionHandler ?? DefaultMotionHandler;
        }

        /// <summary>
        /// Adds a new action to the keystrokes being listened for.
        /// </summary>
        /// <param name="control">The input state to listen for.</param>
        /// <param name="action">The action to perform when said binding is pressed.</param>
        public void AddKeyCommand(ControlMapping control, Action action)
        {
            // TODO: Verify exclusivity and uniqueness with itself and _motions?
            if (!_actions.ContainsKey(control.Key))
                _actions[control.Key] = new List<(ControlMapping control, Action action)>();

            _actions[control.Key].Add((control, action));
        }

        /// <summary>
        /// Removes the binding for a key command.
        /// </summary>
        /// <param name="key">The input state to remove.</param>
        public void RemoveKeyCommand(ControlMapping key)
        {
            var idx = _actions[key.Key].FindIndex(tuple => tuple.control.Matches(key));
            if (idx == -1) throw new ArgumentException("Specified command could not be found.", nameof(key));

            _actions[key.Key].RemoveAt(idx);

            if (_actions[key.Key].Count == 0)
                _actions.Remove(key.Key);
        }

        /// <summary>
        /// Adds a new motion generating keybinding.
        /// </summary>
        /// <param name="control">Input state to use for generating motion.</param>
        /// <param name="direction">Direction to generate when input state is detected.</param>
        public void AddMotion(ControlMapping control, Direction direction)
        {
            // TODO: Verify exclusivity and uniqueness with itself and _actions?
            if (!_motions.ContainsKey(control.Key))
                _motions[control.Key] = new List<(ControlMapping control, Direction direction)>();

            _motions[control.Key].Add((control, direction));
        }

        /// <summary>
        /// Removes the binding for a motion.
        /// </summary>
        /// <param name="key">The input state to remove.</param>
        public void RemoveMotion(ControlMapping key)
        {
            var idx = _motions[key.Key].FindIndex(tuple => tuple.control.Matches(key));
            if (idx == -1) throw new ArgumentException("Specified command could not be found.", nameof(key));

            _motions[key.Key].RemoveAt(idx);

            if (_motions[key.Key].Count == 0)
                _motions.Remove(key.Key);
        }

        /// <inheritdoc />
        public override void ProcessKeyboard(IScreenObject host, Keyboard keyboard, out bool handled)
        {
            // Map any keys pressed to appropriate key bindings
            foreach (var key in keyboard.KeysPressed)
            {
                if (_motions.ContainsKey(key.Key))
                {
                    foreach (var (control, direction) in _motions[key.Key])
                        if (control.ModiferConditionsMet(keyboard))
                        {
                            MotionHandler(direction);
                            handled = true;
                            return;
                        }
                }

                if (_actions.ContainsKey(key.Key))
                {
                    foreach (var (control, action) in _actions[key.Key])
                        if (control.ModiferConditionsMet(keyboard))
                        {
                            action();
                            handled = true;
                            return;
                        }
                }
            }

            handled = false;
        }

        private void DefaultMotionHandler(Direction direction)
        {
            if(Parent!.CanMoveIn(direction))
                Parent!.Position += direction;
        }
    }
}
