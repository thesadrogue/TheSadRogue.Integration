using System;
using GoRogue.Components;
using GoRogue.GameFramework;
using SadRogue.Primitives;

namespace TheSadRogue.Integration
{
    public partial class RogueLikeEntity
    {
        private bool _isTransparent;
        private bool _isWalkable;

        /// <inheritdoc />
        public uint ID { get; }

        /// <inheritdoc />
        public int Layer => ZIndex;

        /// <inheritdoc />
        public Map? CurrentMap { get; private set; }

        /// <inheritdoc />
        public ITaggableComponentCollection GoRogueComponents { get; private set; }

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

        /// <inheritdoc />
        public void OnMapChanged(Map? newMap)
            => CurrentMap = newMap;

        /// <inheritdoc />
        public event EventHandler<GameObjectPropertyChanged<Point>>? Moved;

        /// <inheritdoc />
        public event EventHandler<GameObjectPropertyChanged<bool>>? TransparencyChanged;

        /// <inheritdoc />
        public event EventHandler<GameObjectPropertyChanged<bool>>? WalkabilityChanged;
    }
}
