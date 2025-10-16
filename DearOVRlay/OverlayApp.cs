using System;
using System.Drawing;
using Silk.NET.Maths;
using Silk.NET.OpenGL.Extensions.ImGui;
using Valve.VR;

namespace DearOVRlay;

public class OverlayApp {
    private OVRInputContext input = new();
    private FakeView view;
    public readonly OpenVROverlay overlay;

    public Size Size {
        get => new(view.Size.X, view.Size.Y);
        set => view.Size = new Vector2D<int>(value.Width, value.Height);
    }

    public string ThumbnailImagePath { set => overlay.ThumbnailImagePath = value; }
    public float WidthInMeters { set => overlay.WidthInMeters = value; }
    public event Action<VREvent_t> OnEvent;
    public event Action<double> OnRender;
    
    public OverlayApp(string id, string displayName, int pixelWidth, int pixelHeight) {
        view = new FakeView(pixelWidth, pixelHeight);
        overlay = new(id, displayName) { OverlayMouseScale = view.Size };
        view.Resize += size => overlay.OverlayMouseScale = size;
        
        overlay.OnEvent += evt => {
            if (!input.HandleOVREvent(evt)) 
                OnEvent?.Invoke(evt);
        };
    }

    public void Run() {
        OpenGLRenderer.InitGL(gl => {
            var fb = new OpenGLRenderer(gl, view.Size);
            view.Resize += size => fb.Size = size;
            var imguiController = new ImGuiController(gl, view, input);

            overlay.OnUpdate += delta => {
                overlay.PollEvents();
                imguiController.Update((float)delta);
            };

            overlay.OnRender += delta => {
                fb.Render(() => {
                    OnRender(delta);
                    imguiController.Render();
                });
                overlay.SubmitOpenGLTextureFrame((IntPtr)fb.texture);
            };

            overlay.Run();
        });
    }

    
}