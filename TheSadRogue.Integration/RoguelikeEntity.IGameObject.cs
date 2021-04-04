using System;
using GoRogue.Components;
using GoRogue.GameFramework;
using SadRogue.Primitives;

namespace SadRogue.Integration
{
    public partial class RogueLikeEntity
    {
        private bool _isTransparent;
        private bool _isWalkable;

        /// <inheritdoc />
        public uint ID { get; private set; }

        /// <inheritdoc />
        public int Layer => ZIndex;

        /// <inheritdoc />
        public Map? CurrentMap { get; private set; }

        // Nullable override to suppress warning on constructors; warning is incorrect; the functions that the
        // constructors call initializes this to a non-null value.
        /// <inheritdoc />
        public ITaggableComponentCollection GoRogueComponents { get; private set; } = null!;

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
