using System;
using GoRogue.Components;
using GoRogue.Components.ParentAware;
using JetBrains.Annotations;
using SadConsole;
using SadConsole.Components;
using SadConsole.Input;

namespace SadRogue.Integration.Components
{
    /// <summary>
    /// A Component that works with both SadConsole and GoRogue's GameFramework that may be attached to any
    /// IGameObject or anything that implements IObjectWithComponents, as well as any IScreenObject.
    /// </summary>
    /// <remarks>
    /// You may want to consider <see cref="RogueLikeComponentBase{TParent}"/> so that the Parent field has
    /// a more useful type.
    ///
    /// The intended use is for you to derive from this class and then override the methods you need.
    ///
    /// If adding this component to a <see cref="RogueLikeEntity"/>, <see cref="Maps.RogueLikeMap"/>, or anything else
    /// with an "AllComponents" field, you should attach this component to the AllComponents field ONLY; it will automatically
    /// be attached to SadConsole's components fields as needed.  If your object only has a SadComponents field, you can instead
    /// attach it directly there.  In either case, the Parent field will be updated accordingly.
    /// </remarks>
    [PublicAPI]
    public abstract class RogueLikeComponentBase : RogueLikeComponentBase<object>
    {
        /// <summary>
        /// Creates a new component.
        /// </summary>
        /// <param name="isUpdate">Whether or not this component performs a task OnUpdate</param>
        /// <param name="isRender">Whether or not this component performs a task OnRender</param>
        /// <param name="isMouse">Whether or not this component listens for the mouse</param>
        /// <param name="isKeyboard">Whether or not this component listens for the keyboard</param>
        /// <param name="sortOrder">The order in which this component is processed</param>
        protected RogueLikeComponentBase(bool isUpdate, bool isRender, bool isMouse, bool isKeyboard, uint sortOrder = 5)
            : base(isUpdate, isRender, isMouse, isKeyboard, sortOrder)
        { }
    }

    /// <summary>
    /// A component that works with both SadConsole's and GoRogue's component system.
    /// It is identical to <see cref="RogueLikeComponentBase"/>, except that it ensures that it is always attached
    /// to an object of type <see tparam="TParent"/>, and exposes the Parent property as that type.
    /// </summary>
    /// <typeparam name="TParent">Type of object this component must be attached to.</typeparam>
    [PublicAPI]
    public class RogueLikeComponentBase<TParent> : ParentAwareComponentBase<TParent>, IComponent, ISortedComponent
        where TParent : class
    {
        /// <summary>
        /// The Order in which the components are processed. Lower is earlier.
        /// </summary>
        public uint SortOrder { get; }

        /// <summary>
        /// Whether or not this component performs a task OnUpdate
        /// </summary>
        public bool IsUpdate { get; }

        /// <summary>
        /// Whether or not this component performs a task OnRender
        /// </summary>
        public bool IsRender { get; }

        /// <summary>
        /// Whether or not this component listens for mouse input
        /// </summary>
        public bool IsMouse { get; }

        /// <summary>
        /// Whether or not this component listens to the keyboard
        /// </summary>
        public bool IsKeyboard { get; }

        /// <summary>
        /// Creates a new component
        /// </summary>
        /// <param name="isUpdate">Whether or not this component performs a task OnUpdate</param>
        /// <param name="isRender">Whether or not this component performs a task OnRender</param>
        /// <param name="isMouse">Whether or not this component listens for the mouse</param>
        /// <param name="isKeyboard">Whether or not this component listens for the keyboard</param>
        /// <param name="sortOrder">The order in which this component is processed</param>
        protected RogueLikeComponentBase(bool isUpdate, bool isRender, bool isMouse, bool isKeyboard, uint sortOrder = 5)
        {
            IsUpdate = isUpdate;
            IsRender = isRender;
            IsMouse = isMouse;
            SortOrder = sortOrder;
            IsKeyboard = isKeyboard;
        }

        /// <summary>
        /// The method called OnUpdate, if IsUpdate is true.
        /// </summary>
        /// <param name="host">The "Parent" if you're using RogueLikeEntity</param>
        /// <param name="delta">The difference in time between this call and the previous</param>
        public virtual void Update(IScreenObject host, TimeSpan delta) { }

        /// <summary>
        /// The method called OnRender, if IsRender is true.
        /// </summary>
        /// <param name="host">The "Parent" if you're using RogueLikeEntity</param>
        /// <param name="delta">The difference in time between this call and the previous</param>
        public virtual void Render(IScreenObject host, TimeSpan delta) { }

        /// <summary>
        /// Hook up an event handler to run this when the component is added to an object.  Ensure you
        /// call the base implementation of this function if you override it; its functionality is required
        /// for this object's Parent field to be set correctly.
        /// </summary>
        /// <param name="host">The "Parent" if you're using RogueLikeEntity</param>
        public virtual void OnAdded(IScreenObject host)
        {
            // If the component was added to an AllComponents field (or the GoRogueComponents field)
            // of some object, then the parent will be set correctly by the time this method is called.
            // If it was added to a SadComponents field, then the parent will not be set correctly.
            // We check for that here and set it if needed.
            if (Parent != host)
                Parent = (TParent)host;
        }

        /// <summary>
        /// Hook up an event handler to run this when the component is removed from an object.  Ensure you
        /// call the base implementation of this function if you override it; its functionality is required
        /// for this object's Parent field to be set correctly.
        /// </summary>
        /// <param name="host">The "Parent" if you're using RogueLikeEntity</param>
        public virtual void OnRemoved(IScreenObject host)
        {
            // If the component was removed from an AllComponents field (or the GoRogueComponents field)
            // of some object, then the parent will be set correctly by the time this method is called.
            // If it was removed from a SadComponents field, then the parent will not be set correctly.
            // We check for that here and set it if needed.
            if (Parent == host)
                Parent = null;
        }

        /// <summary>
        /// Called when IsMouse is true
        /// </summary>
        /// <param name="host">The "Parent" if you're using RogueLikeEntity</param>
        /// <param name="state">The state of the mouse</param>
        /// <param name="handled">Whether or not we are done handling mouse input</param>
        public virtual void ProcessMouse(IScreenObject host, MouseScreenObjectState state, out bool handled) => handled = false;

        /// <summary>
        /// Called when IsKeyboard is true
        /// </summary>
        /// <param name="host">The "Parent" if you're using RogueLikeEntity</param>
        /// <param name="keyboard">The state of the keyboard</param>
        /// <param name="handled">Whether or not we are done handling keyboard input</param>
        public virtual void ProcessKeyboard(IScreenObject host, Keyboard keyboard, out bool handled) => handled = false;

    }
}
