using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue.Components;
using GoRogue.GameFramework;
using SadConsole;
using SadConsole.Entities;
using SadRogue.Primitives;
using TheSadRogue.Integration.Components;

namespace TheSadRogue.Integration
{
    /// <summary>
    /// Everything that will be rendered to the screen, except for written text
    /// </summary>
    public class RogueLikeEntity : Entity, IGameObject
    {
        private bool _isTransparent;
        private bool _isWalkable;

        public uint ID { get; }
        public int Layer { get; }
        public Map? CurrentMap { get; private set; }
        public ITaggableComponentCollection GoRogueComponents { get; private set; }

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
        public RogueLikeEntity(Point position, int glyph, bool walkable = true, bool transparent = true, int layer = 0) : this(Color.White, Color.Black, glyph, layer)
        {
            Position = position;
            IsWalkable = walkable;
            IsTransparent = transparent;
            Layer = layer;
        }

        public RogueLikeEntity(Point position, Color foreground, int glyph, int layer = 0) : this(foreground, Color.Black, glyph, layer)
        {
            Position = position;
        }
        public RogueLikeEntity(Color foreground, Color background, int glyph, int layer) : base(foreground, background, glyph, layer)
        {
            IsWalkable = true;
            IsTransparent = false;
            Layer = layer;
            UseMouse = Settings.DefaultScreenObjectUseMouse;
            UseKeyboard = Settings.DefaultScreenObjectUseKeyboard;
            Appearance = new ColoredGlyph(foreground, background, glyph);
            // Moved += SadConsole_Moved;
            // Moved += GoRogue_Moved;
            PositionChanged += Position_Changed;
            GoRogueComponents = new ComponentCollection();
        }
        #endregion
        
        public void OnMapChanged(Map? newMap)
            => CurrentMap = newMap;
        
        #region event handlers
        
        public event EventHandler<GameObjectPropertyChanged<Point>>? Moved;
        public event EventHandler<GameObjectPropertyChanged<bool>>? TransparencyChanged;
        public event EventHandler<GameObjectPropertyChanged<bool>>? WalkabilityChanged;

        private void GoRogue_Moved(object? sender, GameObjectPropertyChanged<Point> change)
        {
            if (Position != change.NewValue && this.CanMove(change.NewValue))
            {
                Position = change.NewValue;
                if (((IScreenObject)this).Position != change.NewValue)
                    Position = change.OldValue;
            }
        }
        private void SadConsole_Moved(object? sender, GameObjectPropertyChanged<Point> change)
        {
            if (Position != change.NewValue && this.CanMove(change.NewValue))
            {
                Position = change.NewValue;
                if (((IGameObject)this).Position != change.NewValue)
                    Position = change.OldValue;
            }
        }

        private void Position_Changed(object? sender, ValueChangedEventArgs<Point> e)
            => Moved?.Invoke(sender, e.ToGameObjectPropertyChanged(this));

        #endregion
        
        #region components
        public void AddComponent(IRogueLikeComponent component)
        {
            // Components.Add(component);
            component.Parent = this;
            SadComponents.Add(component);
            GoRogueComponents.Add(component);
        }
        public void AddComponents(IEnumerable<IRogueLikeComponent> components)
        {
            foreach (var component in components)
                AddComponent(component);

        }

        public T GetComponent<T>(string tag = "")
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
        public IEnumerable<IRogueLikeComponent> GetComponents()
            => GoRogueComponents.GetAll<IRogueLikeComponent>().Concat(GetSadComponents<IRogueLikeComponent>());

        public IEnumerable<T> GetComponents<T>()
        {
            foreach (var component in GetComponents())
            {
                if (component is T rlComponent)
                {
                    yield return rlComponent;
                }
            }
        }


        //todo - RemoveComponent<T>()
        //todo - RemoveComponents(???)
        #endregion
    }
}