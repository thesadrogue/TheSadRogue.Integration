using System.Collections.Generic;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using SadRogue.Primitives;

namespace ExampleGame
{
    public class CryptGenerationStep : GenerationStep
    {
        protected override IEnumerator<object?> OnPerform(GenerationContext context)
        {
            int roomWidth = 13;
            int roomHeight = 6;
            var map = context.GetFirst<ISettableMapView<bool>>();
            var rooms = new List<Rectangle>();

            for (int i = 0; i < map.Width; i += roomWidth)
            {
                for (int j = 0; j < map.Height; j += roomHeight)
                {
                    int offset = j % 2 == 0 ? 0 : roomWidth / 2;
                    rooms.Add(new Rectangle(i + offset, j, roomWidth, roomHeight));
                }
            }
            
            foreach(var room in rooms)
                foreach (var point in room.PerimeterPositions())
                    map[point] = false;
            yield return null;
        }
    }
}