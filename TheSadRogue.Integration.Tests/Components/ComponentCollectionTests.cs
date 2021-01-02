using System.ComponentModel;
using TheSadRogue.Integration.Components;
using Xunit;

namespace TheSadRogue.Integration.Tests.Components
{
    public class ComponentCollectionTests
    {
        private readonly RogueLikeComponentCollection _collection;

        public ComponentCollectionTests()
        {
            _collection = new RogueLikeComponentCollection();
        }

        [Fact]
        public void AddComponentTest()
        {
            Assert.Empty(_collection);
            var component = new PlayerControlsComponent();
            _collection.Add(component);
            Assert.Single(_collection);
        }
        [Fact]
        public void RemoveComponentTest()
        {
            var component = new PlayerControlsComponent();
            _collection.Add(component);

            Assert.Single(_collection);

            _collection.Remove(component);
            
            Assert.Empty(_collection);
        }
        [Fact]
        public void ContainsTest()
        {
            var component = new PlayerControlsComponent();
            _collection.Add(component);

            Assert.Single(_collection);

            Assert.True(_collection.Contains<PlayerControlsComponent>());
        }
        [Fact]
        public void GetTest()
        {
            var component = new PlayerControlsComponent();
            _collection.Add(component);

            Assert.Single(_collection);

            var newComponent = _collection.GetFirst<PlayerControlsComponent>();
            Assert.NotNull(newComponent);
            Assert.Equal(component, newComponent);
        }
    }
}