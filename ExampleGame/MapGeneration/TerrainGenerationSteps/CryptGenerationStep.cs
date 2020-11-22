using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using SadRogue.Primitives;

namespace ExampleGame.MapGeneration.TerrainGenerationSteps
{
    public class CryptGenerationStep : GenerationStep
    {
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            int roomWidth = 13;
            int roomHeight = 6;
            var map = context.GetFirstOrNew<ISettableMapView<bool>>(()=> new ArrayMap<bool>(context.Width, context.Height));
            var rooms = new List<Rectangle>();

            for (int i = 0; i < map.Width; i += roomWidth)
            {
                for (int j = 0; j < map.Height; j += roomHeight)
                {
                    int offset = j % 2 == 0 ? 0 : roomWidth / 2;
                    rooms.Add(new Rectangle(i + offset, j, roomWidth, roomHeight));
                    map[i, j] = true;
                }
            }
            
            foreach(var room in rooms)
            {
                foreach (var point in room.Positions())
                {
                    if (map.Contains(point))
                    {
                        if (room.PerimeterPositions().Contains(point))
                            map[point] = false;
                        else
                            map[point] = true;
                    }
                }
            }
            
            yield return null;
        }
    }
}