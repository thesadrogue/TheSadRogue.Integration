using System.Collections.Generic;
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
        public void RemoveComponentsTest()
        {
            var component = new PlayerControlsComponent();
            _collection.Add(component);

            Assert.Single(_collection);

            _collection.Remove(component);
            
            Assert.Empty(_collection);
        }
        // [Fact]
        // public void HasComponentTest()
        // {
        //     entity = new TheSadRogue.Integration.RogueLikeEntity((0,0), 1);
        //     
        //     Assert.Empty(entity.GetComponents());
        //     
        //     var component = new PlayerControlsComponent();
        //     
        //     entity.AddComponent(component);
        //     Assert.Single(entity.GetComponents());
        // }
        // [Fact]
        // public void HasComponentsTest()
        // {
        //     entity = new TheSadRogue.Integration.RogueLikeEntity((0,0), 1);
        //     
        //     Assert.Empty(entity.GetComponents());
        //     
        //     var component = new PlayerControlsComponent();
        //     
        //     entity.AddComponent(component);
        //     Assert.Single(entity.GetComponents());
        // }
        // [Fact]
        // public void RemoveComponentTest()
        // {
        //     entity = new TheSadRogue.Integration.RogueLikeEntity((0,0), 1);
        //     
        //     Assert.Empty(entity.GetComponents());
        //     
        //     var component = new PlayerControlsComponent();
        //     
        //     entity.AddComponent(component);
        //     Assert.Single(entity.GetComponents());
        // }
        // [Fact]
        // public void RemoveComponentsTest()
        // {
        //     entity = new TheSadRogue.Integration.RogueLikeEntity((0,0), 1);
        //     
        //     Assert.Empty(entity.GetComponents());
        //     
        //     var component = new PlayerControlsComponent();
        //     
        //     entity.AddComponent(component);
        //     Assert.Single(entity.GetComponents());
        // }
    }
}