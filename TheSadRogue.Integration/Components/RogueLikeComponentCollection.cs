using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using GoRogue.Components;
using SadConsole.Components;
using SadConsole.Input;

namespace TheSadRogue.Integration.Components
{
    public class RogueLikeComponentCollection : ObservableCollection<IComponent>, ITaggableComponentCollection
    {
        //all components
        private readonly ComponentCollection _components;
        
        //split into sub-lists to speed-up processing
        private readonly ComponentCollection _componentsUpdate;
        private readonly ComponentCollection _componentsRender;
        private readonly ComponentCollection _componentsMouse;
        private readonly ComponentCollection _componentsKeyboard;
        private readonly ComponentCollection _componentsEmpty;
        
        //event handlers
        public event EventHandler<ComponentChangedEventArgs>? ComponentAdded;
        public event EventHandler<ComponentChangedEventArgs>? ComponentRemoved;

        private IRogueLikeComponent[] _serialized;
        
        public RogueLikeComponentCollection()
        {
            _components = new ComponentCollection();
            _componentsEmpty = new ComponentCollection();
            _componentsKeyboard = new ComponentCollection();
            _componentsMouse = new ComponentCollection();
            _componentsRender = new ComponentCollection();
            _componentsUpdate = new ComponentCollection();

            //ComponentAdded += OnAdded;
            //ComponentRemoved += OnRemoved;
            CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newComponent in e.NewItems)
                        Add(newComponent);
                    
                    break;
                
                case NotifyCollectionChangedAction.Remove:
                    foreach (object oldComponent in e.OldItems)
                        Remove(oldComponent);
                    break;
                
                case NotifyCollectionChangedAction.Replace:
                    foreach (object newComponent in e.NewItems)
                        Add(newComponent);
                    
                    foreach (object oldComponent in e.OldItems)
                        Remove(oldComponent);
                    
                    break;
                
                case NotifyCollectionChangedAction.Reset:
                    while (_components.Any())
                        Remove(_componentsRender.GetFirst<IRogueLikeComponent>());
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public void Add<T>(T component) where T : class
        {
            if (component is IRogueLikeComponent rlComponent)
            {
                if (!Contains(rlComponent))
                {
                    _components.Add(rlComponent);
                    base.Add(rlComponent);
                    if(rlComponent.IsKeyboard)
                        _componentsKeyboard.Add(component);
                
                    if(rlComponent.IsMouse)
                        _componentsMouse.Add(component);
                
                    if(rlComponent.IsRender)
                        _componentsRender.Add(component);
                
                    if(rlComponent.IsUpdate)
                        _componentsUpdate.Add(component);
                
                    else if(!rlComponent.IsKeyboard && !rlComponent.IsMouse && !rlComponent.IsRender)
                        _componentsEmpty.Add(component);
                }
            }
        }
        public void Add<T>(T component, string? tag = null) where T : class => Add(component);
        public IEnumerable<T> GetAll<T>() where T : class => _components.GetAll<T>();
        public IEnumerator<ComponentTagPair> GetEnumerator() => _components.GetEnumerator();
        public T GetFirst<T>() where T : class => _components.GetFirst<T>();
        public T GetFirst<T>(string? tag = null) where T : class => _components.GetFirst<T>(tag);
        public T GetFirstOrDefault<T>() where T : class => _components.GetFirstOrDefault<T>();
        public T GetFirstOrDefault<T>(string? tag = null) where T : class => _components.GetFirstOrDefault<T>(tag);
        public bool Contains<T>() where T : class => _components.Contains<T>();
        public bool Contains<T>(string? tag = null) where T : class => _components.Contains<T>(tag);
        public bool Contains(Type componentType) => _components.Contains(componentType);
        public bool Contains(params ComponentTypeTagPair[] componentTypesAndTags) => _components.Contains(componentTypesAndTags);
        public bool Contains(Type componentType, string? tag = null) => _components.Contains(componentType, tag);
        public bool Contains(params Type[] componentTypes) => _components.Contains(componentTypes);
        public void Remove(string tag)
        {
            if (_components.Contains<IRogueLikeComponent>(tag))
                Remove(_components.GetFirst<IRogueLikeComponent>(tag));
        }
        
        public void Remove(params string[] tags)
        {
            foreach (var tag in tags)
                Remove(tag);
        }

        public void Remove(object component)
        {
            if (component is IRogueLikeComponent rlComponent)
            {
                if (Contains(rlComponent))
                {
                    base.Remove(rlComponent);
                    _components.Remove(rlComponent);
                    
                    if(rlComponent.IsKeyboard)
                        _componentsKeyboard.Remove(rlComponent);
                
                    if(rlComponent.IsMouse)
                        _componentsMouse.Remove(rlComponent);
                
                    if(rlComponent.IsRender)
                        _componentsRender.Remove(rlComponent);
                
                    if(rlComponent.IsUpdate)
                        _componentsUpdate.Remove(rlComponent);
                
                    else if(!rlComponent.IsKeyboard && !rlComponent.IsMouse && !rlComponent.IsRender)
                        _componentsEmpty.Remove(rlComponent);
                }
            }
        }

        public void Remove(params object[] components)
        {
            foreach (var component in components)
                Remove(component);
        }

        public void Render(in TimeSpan delta)
        {
            foreach (var componentTagPair in _componentsRender)
            {
                var component = (IRogueLikeComponent)componentTagPair.Component;
                component.Render((RogueLikeEntity)component.Parent, delta);
            }
        }

        public void Update(in TimeSpan delta)
        {
            foreach (var componentTagPair in _componentsUpdate)
            {
                var component = (IRogueLikeComponent)componentTagPair.Component;
                component.Update((RogueLikeEntity)component.Parent, delta);
            }        
        }

        public bool ProcessMouse(MouseScreenObjectState state)
        {
            bool handled = false;
            foreach (var componentTagPair in _componentsMouse)
            {
                var component = (IRogueLikeComponent)componentTagPair.Component;
                component.ProcessMouse((RogueLikeEntity)component.Parent, state, out handled);
            }

            return handled;
        }

        public bool ProcessKeyboard(Keyboard keyboard)
        {
            bool handled = false;
            foreach (var componentTagPair in _componentsKeyboard)
            {
                var component = (IRogueLikeComponent)componentTagPair.Component;
                component.ProcessKeyboard((RogueLikeEntity)component.Parent, keyboard, out handled);
            }

            return handled;
        }
    }
}