using System;
using Silk.NET.Core.Contexts;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace DearOVRlay;

public class FakeView : IView {
    public event Action<Vector2D<int>>? Resize;
    private Vector2D<int> _Size;

    public Vector2D<int> Size {
        get => _Size;
        set {
            _Size = value;
            Resize?.Invoke(value);
        }
    }

    public Vector2D<int> FramebufferSize => Size;

    public FakeView(int w, int h) {
        _Size = new Vector2D<int>(w, h);
    }
    
    
    // things we need to satisfy IView but aren't actually used by ImGuiController
    public bool ShouldSwapAutomatically { get; set; }
    public bool IsEventDriven { get; set; }
    public bool IsContextControlDisabled { get; set; }
    public double FramesPerSecond { get; set; }
    public double UpdatesPerSecond { get; set; }
    public GraphicsAPI API { get; }
    public bool VSync { get; set; }
    public VideoMode VideoMode { get; }
    public int? PreferredDepthBufferBits { get; }
    public int? PreferredStencilBufferBits { get; }
    public Vector4D<int>? PreferredBitDepth { get; }
    public int? Samples { get; }
    public IGLContext? GLContext { get; }
    public IVkSurface? VkSurface { get; }
    public void Dispose() {
        throw new NotImplementedException();
    }
    public INativeWindow? Native { get; }
    public void Initialize() {
        throw new NotImplementedException();
    }
    public void DoRender() {
        throw new NotImplementedException();
    }
    public void DoUpdate() {
        throw new NotImplementedException();
    }
    public void DoEvents() {
        throw new NotImplementedException();
    }
    public void ContinueEvents() {
        throw new NotImplementedException();
    }
    public void Reset() {
        throw new NotImplementedException();
    }
    public void Focus() {
        throw new NotImplementedException();
    }
    public void Close() {
        throw new NotImplementedException();
    }
    public Vector2D<int> PointToClient(Vector2D<int> point) {
        throw new NotImplementedException();
    }
    public Vector2D<int> PointToScreen(Vector2D<int> point) {
        throw new NotImplementedException();
    }
    public Vector2D<int> PointToFramebuffer(Vector2D<int> point) {
        throw new NotImplementedException();
    }
    public object Invoke(Delegate d, params object[] args) {
        throw new NotImplementedException();
    }
    public void Run(Action onFrame) {
        throw new NotImplementedException();
    }
    public IntPtr Handle { get; }
    public bool IsClosing { get; }
    public double Time { get; }
    public bool IsInitialized { get; }
    public event Action<Vector2D<int>>? FramebufferResize;
    public event Action? Closing;
    public event Action<bool>? FocusChanged;
    public event Action? Load;
    public event Action<double>? Update;
    public event Action<double>? Render;
}