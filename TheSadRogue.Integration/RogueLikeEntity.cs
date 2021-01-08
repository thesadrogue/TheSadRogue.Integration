using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue.Components;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SadConsole;
using SadConsole.Components;
using SadConsole.Entities;
using SadRogue.Primitives;

namespace TheSadRogue.Integration
{
    public class RogueLikeEntity : Entity, IGameObject
    {
        private bool _isTransparent;
        private bool _isWalkable;

        public uint ID { get; }
        public int Layer { get; }
        public Map? CurrentMap { get; private set; }
        public ITaggableComponentCollection GoRogueComponents { get; private set; }
        /// <summary>
        /// Each and every component on this entity
        /// </summary>
        /// <remarks>
        /// Confused about which collection to add a component to?
        /// Add it here.
        /// </remarks>
        public ITaggableComponentCollection AllComponents => GoRogueComponents;

        /// <inheritdoc />
        Point IGameObject.Position
        {
            get => base.Position;
            set => base.Position = value;
        }

        /// <inheritdoc />
        public bool IsTransparent
        {
            get => _isTransparent;
            set => this.SafelySetProperty(ref _isTransparent, value, TransparencyChanged);
        }        
        
        /// <inheritdoc />
        public bool IsWalkable
        {
            get => _isWalkable;
            set => this.SafelySetProperty(ref _isWalkable, value, WalkabilityChanged);
        }

        #region initialization
        public RogueLikeEntity(Point position, int glyph, bool walkable = true, bool transparent = true, int layer = 0) 
            : this(position, Color.White, Color.Black, glyph, walkable, transparent, layer)
        { }

        public RogueLikeEntity(Point position, Color foreground, int glyph, bool walkable = true, bool transparent = true, int layer = 0) 
            : this(position, foreground, Color.Black, glyph, walkable, transparent, layer)
        { }
        
        public RogueLikeEntity(Point position, Color foreground, Color background, int glyph, bool walkable = true, bool transparent = true, int layer = 0) 
            : base(foreground, background, glyph, layer)
        {            
            Position = position;
            PositionChanged += Position_Changed;
            
            Appearance = new ColoredGlyph(foreground, background, glyph);
            
            IsWalkable = walkable;
            IsTransparent = transparent;
            
            Layer = layer;
            
            GoRogueComponents = new ComponentCollection();
            GoRogueComponents.ComponentAdded += On_GoRogueComponentAdded;
            GoRogueComponents.ComponentRemoved += On_GoRogueComponentRemoved;
        }
        #endregion
        
        public void OnMapChanged(Map? newMap)
            => CurrentMap = newMap;
        
        #region event handlers
        
        public event EventHandler<GameObjectPropertyChanged<Point>>? Moved;
        public event EventHandler<GameObjectPropertyChanged<bool>>? TransparencyChanged;
        public event EventHandler<GameObjectPropertyChanged<bool>>? WalkabilityChanged;

        public void On_GoRogueComponentAdded(object? s, ComponentChangedEventArgs e)
        {
            if (e.Component is IComponent sadComponent)
                SadComponents.Add(sadComponent);
        }

        public void On_GoRogueComponentRemoved(object? s, ComponentChangedEventArgs e)
        {
            if (e.Component is IComponent sadComponent)
                SadComponents.Remove(sadComponent);
        }

        private void Position_Changed(object? sender, ValueChangedEventArgs<Point> e)
            => Moved?.Invoke(sender, new GameObjectPropertyChanged<Point>(this, e.OldValue, e.NewValue));

        #endregion
        
        #region components
        public void AddComponent(object component, string tag = null)
        {
            if (component is IGameObjectComponent goc)
                goc.Parent = this;
            
            GoRogueComponents.Add(component, tag);
        }
        public void AddComponents(IEnumerable<object> components)
        {
            foreach (var component in components)
                AddComponent(component);
        }

        public T GetComponent<T>(string tag = null)
        {
            //temporary
            // if (tag is "")
            // {
            return GetComponents<T>().Distinct().FirstOrDefault();
            // }
            // else
            // {
            //     return GetComponents<T>().Distinct().FirstOrDefault();
            // }
        }
        
        //public T GetComponent<T>() => GoRogueComponents.GetFirst<T>();
        
        public IEnumerable<T> GetComponents<T>()
        {
            foreach (var component in GoRogueComponents)
                if (component.Component is T rlComponent)
                    yield return rlComponent;
        }
        
        //todo - RemoveComponent<T>()
        //todo - RemoveComponents(???)
        #endregion
    }
}