using System;
using GoRogue.Components;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using GoRogue.Random;
using SadConsole;
using SadConsole.Components;
using SadConsole.Entities;
using SadRogue.Primitives;

namespace TheSadRogue.Integration
{
    public partial class RogueLikeEntity : Entity, IGameObject
    {
        /// <summary>
        /// Each and every component on this entity
        /// </summary>
        /// <remarks>
        /// Confused about which collection to add a component to?
        /// Add it here.
        /// </remarks>
        public ITaggableComponentCollection AllComponents => GoRogueComponents;

        #region Initialization
        public RogueLikeEntity(Point position, int glyph, bool walkable = true, bool transparent = true, int layer = 1, Func<uint>? idGenerator = null, ITaggableComponentCollection? customComponentContainer = null)
            : this(position, Color.White, Color.Black, glyph, walkable, transparent, layer, idGenerator, customComponentContainer)
        { }

        public RogueLikeEntity(Point position, Color foreground, int glyph, bool walkable = true, bool transparent = true, int layer = 1, Func<uint>? idGenerator = null, ITaggableComponentCollection? customComponentContainer = null)
            : this(position, foreground, Color.Black, glyph, walkable, transparent, layer, idGenerator, customComponentContainer)
        { }

        public RogueLikeEntity(Point position, Color foreground, Color background, int glyph, bool walkable = true, bool transparent = true, int layer = 1, Func<uint>? idGenerator = null, ITaggableComponentCollection? customComponentContainer = null)
            : base(foreground, background, glyph, layer != 0 ? layer : throw new ArgumentException($"{nameof(RogueLikeEntity)} objects may not reside on the terrain layer.", nameof(layer)))
        {
            idGenerator ??= GlobalRandom.DefaultRNG.NextUInt;

            Position = position;
            PositionChanged += Position_Changed;

            IsWalkable = walkable;
            IsTransparent = transparent;

            ID = idGenerator();
            GoRogueComponents = customComponentContainer ?? new ComponentCollection();
            AllComponents.ComponentAdded += On_GoRogueComponentAdded;
            AllComponents.ComponentRemoved += On_GoRogueComponentRemoved;
        }
        #endregion

        #region Synchronization Handlers

        private void On_GoRogueComponentAdded(object? s, ComponentChangedEventArgs e)
        {
            if (e.Component is IComponent sadComponent)
                SadComponents.Add(sadComponent);
            if (e.Component is IGameObjectComponent goRogueComponent)
            {
                if (goRogueComponent.Parent != null)
                    throw new ArgumentException(
                        $"Components implementing {nameof(IGameObjectComponent)} cannot be added to multiple objects at once.");

                goRogueComponent.Parent = this;
            }
        }

        private void On_GoRogueComponentRemoved(object? s, ComponentChangedEventArgs e)
        {
            if (e.Component is IComponent sadComponent)
                SadComponents.Remove(sadComponent);

            if (e.Component is IGameObjectComponent goRogueComponent)
            {
                goRogueComponent.Parent = null;
            }
        }

        private void Position_Changed(object? sender, ValueChangedEventArgs<Point> e)
            => Moved?.Invoke(sender, new GameObjectPropertyChanged<Point>(this, e.OldValue, e.NewValue));

        #endregion
    }
}
