using SadConsole;
using SadConsole.Renderers;

namespace TheSadRogue.Integration.Tests.Mocks
{
    public class MockRenderStep : IRenderStep
    {
        public void Dispose()
        { }

        public void SetData(object data)
        { }

        public void Reset() => throw new System.NotImplementedException();

        public bool Refresh(IRenderer renderer, IScreenSurface screenObject, bool backingTextureChanged, bool isForced) => throw new System.NotImplementedException();

        public void Composing(IRenderer renderer, IScreenSurface screenObject) => throw new System.NotImplementedException();

        public void Render(IRenderer renderer, IScreenSurface screenObject) => throw new System.NotImplementedException();

        public uint SortOrder { get; set; }
    }
}
