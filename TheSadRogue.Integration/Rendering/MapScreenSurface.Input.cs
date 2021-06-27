// SadConsole does not use nullable reference types and for all intents and purposes this class is a copy-paste
// with trivial modification; so we disable nullable reference types for it to ensure consistent interfaces.
#nullable disable

using System;
using System.Runtime.Serialization;
using SadConsole.Input;

namespace SadRogue.Integration.Rendering
{
    public partial class MapScreenSurface
    {
        /// <inheritdoc/>
        public event EventHandler<MouseScreenObjectState> MouseButtonClicked;

        /// <inheritdoc/>
        public event EventHandler<MouseScreenObjectState> MouseMove;

        /// <inheritdoc/>
        public event EventHandler<MouseScreenObjectState> MouseExit;

        /// <inheritdoc/>
        public event EventHandler<MouseScreenObjectState> MouseEnter;

        /// <summary>
        /// Indicates that the mouse is currently over this console.
        /// </summary>
        protected bool IsMouseOver;

        /// <inheritdoc/>
        [DataMember]
        public bool MoveToFrontOnMouseClick { get; set; }

        /// <inheritdoc/>
        [DataMember]
        public bool FocusOnMouseClick { get; set; }

        /// <summary>
        /// Raises the <see cref="MouseEnter"/> event.
        /// </summary>
        /// <param name="state">Current mouse state in relation to this console.</param>
        protected virtual void OnMouseEnter(MouseScreenObjectState state) => MouseEnter?.Invoke(_surfaceOwningObject, state);

        /// <summary>
        /// Raises the <see cref="MouseExit"/> event.
        /// </summary>
        /// <param name="state">Current mouse state in relation to this console.</param>
        protected virtual void OnMouseExit(MouseScreenObjectState state)
        {
            // Force mouse off just in case
            IsMouseOver = false;

            MouseExit?.Invoke(_surfaceOwningObject, state);
        }

        /// <summary>
        /// Raises the <see cref="MouseMove"/> event.
        /// </summary>
        /// <param name="state">Current mouse state in relation to this console.</param>
        protected virtual void OnMouseMove(MouseScreenObjectState state)
        {
            if (state.Mouse.LeftButtonDown)
            {
                if (MoveToFrontOnMouseClick && Parent != null && Parent.Children.IndexOf(OwningObject) != Parent.Children.Count - 1)
                {
                    Parent.Children.MoveToTop(OwningObject);
                }

                if (FocusOnMouseClick && !IsFocused)
                {
                    IsFocused = true;
                }
            }

            MouseMove?.Invoke(_surfaceOwningObject, state);
        }

        /// <summary>
        /// Raises the <see cref="MouseButtonClicked"/> event. Possibly moves the console to the top of it's parent's children collection.
        /// </summary>
        /// <param name="state">Current mouse state in relation to this console.</param>
        protected virtual void OnMouseLeftClicked(MouseScreenObjectState state) => MouseButtonClicked?.Invoke(_surfaceOwningObject, state);

        /// <summary>
        /// Raises the <see cref="MouseButtonClicked"/> event.
        /// </summary>
        /// <param name="state">Current mouse state in relation to this console.</param>
        protected virtual void OnRightMouseClicked(MouseScreenObjectState state) => MouseButtonClicked?.Invoke(_surfaceOwningObject, state);

        /// <inheritdoc/>
        public override void LostMouse(MouseScreenObjectState state)
        {
            if (IsMouseOver)
            {
                OnMouseExit(state);
            }
        }

        /// <inheritdoc/>
        public override bool ProcessMouse(MouseScreenObjectState state)
        {
            if (!IsVisible) return false;

            foreach (SadConsole.Components.IComponent component in ComponentsMouse.ToArray())
            {
                component.ProcessMouse(OwningObject, state, out bool isHandled);

                if (isHandled)
                    return true;
            }

            if (!UseMouse) return false;

            if (state.IsOnScreenObject)
            {
                if (IsMouseOver != true)
                {
                    IsMouseOver = true;
                    OnMouseEnter(state);
                }

                OnMouseMove(state);

                if (state.Mouse.LeftClicked)
                {
                    OnMouseLeftClicked(state);
                }

                if (state.Mouse.RightClicked)
                {
                    OnRightMouseClicked(state);
                }

                return true;
            }

            if (IsMouseOver)
            {
                IsMouseOver = false;
                OnMouseExit(state);
            }

            return false;
        }
    }
}
