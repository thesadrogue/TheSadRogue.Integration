using SadConsole;
using SadRogue.Primitives;

namespace TheSadRogue.Integration.Templates.MonoGame
{
    /// <summary>
    /// A very basic SadConsole Console subclass that acts as a game message log.
    /// </summary>
    public class MessageLogConsole : Console
    {
        private string _lastMessage;
        private int _lastMessageCount;

        public MessageLogConsole(int width, int height)
            : base(width, height)
        {
            Initialize();
        }

        public MessageLogConsole(int width, int height, int bufferWidth, int bufferHeight)
            : base(width, height, bufferWidth, bufferHeight)
        {
            Initialize();
        }

        public MessageLogConsole(ICellSurface surface, IFont font = null, Point? fontSize = null)
            : base(surface, font, fontSize)
        {
            Initialize();
        }

        private void Initialize()
        {
            Cursor.AutomaticallyShiftRowsUp = true;
            _lastMessage = "";
        }

        public void AddMessage(string message)
        {
            if (_lastMessage == message)
                _lastMessageCount++;
            else
            {
                _lastMessageCount = 1;
                _lastMessage = message;
            }

            if (_lastMessageCount > 1)
            {
                Cursor.Position = Cursor.Position.Translate(0, -1);
                Cursor.Print($"{_lastMessage} (x{_lastMessageCount})");
            }
            else
                Cursor.Print(_lastMessage);

            Cursor.NewLine();
        }
    }
}
