using System;
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

        public void Reset() => throw new NotImplementedException();

        public bool Refresh(IRenderer renderer, IScreenSurface screenObject, bool backingTextureChanged, bool isForced)
            => throw new NotImplementedException();

        public void Composing(IRenderer renderer, IScreenSurface screenObject) => throw new NotImplementedException();

        public void Render(IRenderer renderer, IScreenSurface screenObject) => throw new NotImplementedException();

        public uint SortOrder
        {
            get => 1;
            set => throw new NotImplementedException();
        }
    }
}
