using System;
using System.Collections.Generic;
using System.Linq;
using GoRogue.Components;
using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using GoRogue.SpatialMaps;
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
        public bool IsTransparent { get; set; }
        public bool IsWalkable { get; set; }
        
        public uint ID { get; }
        public int Layer { get; }
        public Map? CurrentMap { get; private set; }

        public RogueLikeComponentCollection Components => SadComponents as RogueLikeComponentCollection;
        public ITaggableComponentCollection GoRogueComponents => Components;

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
            IsTransparent = true;
            UseMouse = Settings.DefaultScreenObjectUseMouse;
            UseKeyboard = Settings.DefaultScreenObjectUseKeyboard;
            Appearance = new ColoredGlyph(foreground, background, glyph);
            SadComponents = new RogueLikeComponentCollection();
            Layer = layer;
            Moved += SadConsole_Moved;
            Moved += GoRogue_Moved;

            Components.ComponentAdded += Component_Added;
            Components.ComponentRemoved += Component_Removed;
        }
        #endregion
        
        #region motion
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

        public bool CanMove(Point position) => CurrentMap!.GetTerrainAt(position)!.IsWalkable;
        public bool CanMoveIn(Direction direction) => CanMove(Position + direction);

        #endregion
        
        #region event handlers
        
        public event EventHandler<GameObjectPropertyChanged<Point>>? Moved;
        public event EventHandler<GameObjectPropertyChanged<bool>>? TransparencyChanged;
        public event EventHandler<GameObjectPropertyChanged<bool>>? WalkabilityChanged;
        private void GoRogue_Moved(object? sender, GameObjectPropertyChanged<Point> change)
        {
            if (Position != change.NewValue)
            {
                Position = change.NewValue;
                if (((IScreenObject)this).Position != change.NewValue)
                    Position = change.OldValue;
            }
        }
        private void SadConsole_Moved(object? sender, GameObjectPropertyChanged<Point> change)
        {
            if (Position != change.NewValue)
            {
                Position = change.NewValue;
                if (((IGameObject)this).Position != change.NewValue)
                    Position = change.OldValue;
            }
        }
        private void Component_Added(object? s, ComponentChangedEventArgs e)
        {
            if (!(e.Component is IGameObjectComponent c))
                return;

            if (c.Parent != null)
                throw new ArgumentException(
                    $"Components implementing {nameof(IGameObjectComponent)} cannot be added to multiple objects at once.");

            c.Parent = this;
        }
        
        private void Component_Removed(object? s, ComponentChangedEventArgs e)
        {
            if (e.Component is IGameObjectComponent c)
                c.Parent = null;
        }
        #endregion
        
        #region components
        public void AddComponent(IRogueLikeComponent component)
        {
            Components.Add(component);
            // SadComponents.Add(component);
            // GoRogueComponents.Add(component);
        }
        public void AddComponents(IEnumerable<IRogueLikeComponent> components)
        {
            Components.Add(components);
            // foreach (var component in components)
            //     SadComponents.Add(component);
            //
            // GoRogueComponents.Add(components);
        }

        public T GetComponent<T>(string tag = "")
        {
            if (tag is "")
            {
                return GetComponents<T>().Distinct().FirstOrDefault();
            }
            else
            {
                //temporary
                return GetComponents<T>().Distinct().FirstOrDefault();
            }
        }
        public IEnumerable<IRogueLikeComponent> GetComponents()
            => GetGoRogueComponents().Concat(GetSadComponents<IRogueLikeComponent>());

        public IEnumerable<T> GetComponents<T>()
        {
            foreach (var component in Components)
            {
                if (component is T rlComponent)
                {
                    yield return rlComponent;
                }
            }
        }
        private IEnumerable<IRogueLikeComponent> GetGoRogueComponents()
        {
            foreach (var pair in GoRogueComponents)
            {
                yield return (IRogueLikeComponent)pair.Component;
            }
        }

        public bool HasComponent<T>() => Components.Contains(typeof(T));

        //todo - RemoveComponent<T>()
        //todo - RemoveComponents(???)
        //
        // public IEnumerable<TComponent> GetSadComponents<TComponent>() where TComponent : class, IComponent
        // {
        //     foreach (IComponent component in SadComponents)
        //     {
        //         if (component is TComponent tComponent)
        //         {
        //             yield return tComponent;
        //         }
        //     }
        // }
        //
        // public TComponent GetSadComponent<TComponent>() where TComponent : class, IComponent
        // {
        //     foreach (IComponent component in SadComponents)
        //     {
        //         if (component is TComponent)
        //             return (TComponent)component;
        //     }
        //
        //     return null;
        // }
        //
        // public bool HasSadComponent<TComponent>(out TComponent component)
        //     where TComponent: class, IComponent
        // {
        //     foreach (IComponent comp in SadComponents)
        //     {
        //         if (comp is TComponent)
        //         {
        //             component = (TComponent)comp;
        //             return true;
        //         }
        //     }
        //
        //     component = null;
        //     return false;
        // }
        // public void SortComponents()
        // {
        //     
        // }

        // static int CompareComponent(IComponent left, IComponent right)
        // {
        //     if (left.SortOrder > right.SortOrder)
        //         return 1;
        //
        //     if (left.SortOrder < right.SortOrder)
        //         return -1;
        //
        //     return 0;
        // }

        #endregion
    }
}