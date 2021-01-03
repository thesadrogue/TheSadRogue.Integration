using GoRogue.GameFramework;
using SadConsole;

namespace TheSadRogue.Integration
{
    public static class EventArgExtensions
    {
        public static GameObjectPropertyChanged<T> ToGameObjectPropertyChanged<T>(this ValueChangedEventArgs<T> args, IGameObject item)  
            => new GameObjectPropertyChanged<T>(item, args.OldValue, args.NewValue);
        
        public static ValueChangedEventArgs<T> ToValueChangedEventArgs<T>(this GameObjectPropertyChanged<T> args)
            => new ValueChangedEventArgs<T>(args.OldValue, args.NewValue);
    }
}