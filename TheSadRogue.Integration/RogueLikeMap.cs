using System.Collections.Generic;
using System.Linq;
using GoRogue.GameFramework;
using SadRogue.Primitives.GridViews;
using GoRogue.SpatialMaps;
using SadConsole;
using SadConsole.Entities;
using SadRogue.Primitives;

namespace TheSadRogue.Integration
{
    /// <summary>
    /// A Map that contains the necessary function to render to a ScreenSurface
    /// </summary>
    public class RogueLikeMap : Map
    {
        /// <summary>
        /// An IGridView of the ColoredGlyphs on the Terrain layer (0)
        /// </summary>
        public IGridView<ColoredGlyph> TerrainView
            => new LambdaTranslationGridView<IGameObject, ColoredGlyph>(Terrain, val => ((RogueLikeEntity)val).Appearance);

        /// <summary>
        /// An IEnumerable of the ColoredGlyphs on the Terrain layer (0)
        /// </summary>
        public IEnumerable<ColoredGlyph> TerrainCells
        {
            get
            {
                var view = TerrainView;
                foreach(var position in view.Positions())
                    yield return view[position];
            }
        }
        
        public Renderer EntityRenderer { get; }

        /// <summary>
        /// Creates a new RogueLikeMap
        /// </summary>
        /// <param name="width">Desired width of map</param>
        /// <param name="height">Desired Height of Map</param>
        /// <param name="numberOfEntityLayers">How many entity layers to include</param>
        /// <param name="distanceMeasurement">How to measure the distance of a single-tile movement</param>
        /// <param name="layersBlockingWalkability">Layers which should factor into move logic</param>
        /// <param name="layersBlockingTransparency">Layers which should factor into transparency</param>
        /// <param name="entityLayersSupportingMultipleItems">How many entity layers support multiple entities per layer</param>
        public RogueLikeMap(int width, int height, int numberOfEntityLayers, Distance distanceMeasurement,
            uint layersBlockingWalkability = 4294967295, uint layersBlockingTransparency = 4294967295,
            uint entityLayersSupportingMultipleItems = 0) : base(width, height, numberOfEntityLayers,
            distanceMeasurement, layersBlockingWalkability, layersBlockingTransparency,
            entityLayersSupportingMultipleItems)
        {
            Entities.ItemAdded += Entity_Added;
            EntityRenderer = new Renderer();
            EntityRenderer.DoEntityUpdate = true;
        }

        /// <summary>
        /// Invoked when an entity is added via Map.AddEntity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void Entity_Added(object? sender, ItemEventArgs<IGameObject> eventArgs)
        {
            if (eventArgs.Item is RogueLikeEntity entity) 
                EntityRenderer.Add(entity);
        }

    }
}