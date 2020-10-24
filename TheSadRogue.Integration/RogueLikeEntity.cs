using System;
using System.Collections.Generic;
using GoRogue.Components;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using GoRogue.SpatialMaps;
using SadConsole;
using SadRogue.Primitives;

namespace TheSadRogue.Integration
{
    /// <summary>
    /// Everything that will be rendered to the screen, except for written text
    /// </summary>
    public class RogueLikeEntity : SadConsole.Entities.Entity, IGameObject
    {
        public uint ID { get; }
        public int Layer { get; }
        public Map? CurrentMap { get; private set; }
        public bool IsTransparent { get; set; }
        public ITaggableComponentCollection GoRogueComponents { get; private set; }
        public bool IsWalkable { get; set; }
        public event EventHandler<GameObjectPropertyChanged<bool>>? TransparencyChanged;
        public event EventHandler<GameObjectPropertyChanged<bool>>? WalkabilityChanged;
        public event EventHandler<GameObjectPropertyChanged<Point>>? Moved;

        public ColoredGlyph Glyph
        {
            get => _animation.CurrentFrame.GetCellAppearance(0, 0);
            set => _animation.CurrentFrame.SetCellAppearance(0, 0, value);
        }
        public ColoredGlyph ToColoredGlyph() => Glyph;
        
        public RogueLikeEntity(Point position, int glyph, bool walkable = true, bool transparent = true, int layer = 0) : base(Color.White, Color.Black, glyph)
        {
            Position = position;
            IsWalkable = walkable;
            IsTransparent = transparent;
            Layer = layer;
            Initialize();
        }

        public RogueLikeEntity(Point position, Color foreground, int glyph) : base(foreground, Color.Black, glyph)
        {
            Position = position;
            Initialize();
        }
        public RogueLikeEntity(Color foreground, Color background, int glyph) : base(foreground, background, glyph)
        {
            Initialize();
        }
        private void Initialize()
        {
            GoRogueComponents =  new ComponentCollection();
            GoRogueComponents.ComponentAdded += On_ComponentAdded;
            GoRogueComponents.ComponentRemoved += On_ComponentRemoved;
            Moved += SadConsole_Moved;
            Moved += GoRogue_Moved;
        }

        public void OnMapChanged(Map? newMap)
        {
            if (newMap != null)
            {
                if (Layer == 0)
                {
                    if (newMap.Terrain[Position] != this)
                    {
                        return; //do nothing
                    }
                }
                else if (!newMap.Entities.Contains(this)) // It's an entity
                {
                    return;//do nothing
                }
            }
            CurrentMap = newMap;
        }

        public bool CanMove(Point position) => CurrentMap.GetTerrainAt(position).IsWalkable;
        public bool CanMoveIn(Direction direction) => CanMove(Position + direction);

        #region event handlers
        // Handle the case where GoRogue's Position property was the one that initiated the move
        private void GoRogue_Moved(object? sender, GameObjectPropertyChanged<Point> gameObjectPropertyChanged)
        {
            if (Position != base.Position) // We need to sync entity
                base.Position = Position;
        }

        // Handle the case where SadConsole's Position property was the one that initiated the move
        private void SadConsole_Moved(object? sender, GameObjectPropertyChanged<Point> change)
        {
            if (Position != change.NewValue)
            {
                Position = change.NewValue;

                // In this case, GoRogue wouldn't allow the position set, so set SadConsole's position back to the way it was
                // to keep them in sync.  Since GoRogue's position never changed, this won't infinite loop.
                if (((IGameObject)this).Position != change.NewValue)
                    Position = change.OldValue;
            }
        }
        
        //from GameObject
        private void On_ComponentAdded(object? s, ComponentChangedEventArgs e)
        {
            if (!(e.Component is IGameObjectComponent c))
                return;

            if (c.Parent != null)
                throw new ArgumentException(
                    $"Components implementing {nameof(IGameObjectComponent)} cannot be added to multiple objects at once.");

            c.Parent = this;
        }
        //from GameObject
        private void On_ComponentRemoved(object? s, ComponentChangedEventArgs e)
        {
            if (e.Component is IGameObjectComponent c)
                c.Parent = null;
        }
        
        
        #endregion
        #region components

        public void AddComponent(IRogueLikeComponent component)
        {
            SadComponents.Add(component);
            GoRogueComponents.Add(component);
        }
        public void AddComponents(IEnumerable<IRogueLikeComponent> components)
        {
            foreach (var component in components)
                SadComponents.Add(component);
            
            GoRogueComponents.Add(components);
        }
        
        //todo - HasComponent<T>()
        //todo - HasComponents(???)
        //todo - HasComponent(string name)
        //todo - GetComponent<T>()
        //todo - GetComponents(???)
        //todo - RemoveComponent<T>()
        //todo - RemoveComponents(???)
        #endregion
    }
}