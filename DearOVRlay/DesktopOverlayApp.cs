using System.Drawing;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace DearOVRlay;

public class DesktopOverlayApp : OverlayApp {
    public new OpenVROverlay Overlay => null;
    private IWindow view;

    public new string ThumbnailImagePath { set { } }
    public new float WidthInMeters { set { }  }
    
    public DesktopOverlayApp(string id = "UNUSED") {
    }

    public new void Run() {
        OpenGLRenderer.InitWindowed(win => {
            view = win;
            view.Resize += vec => this.Size = new Size(vec.X, vec.Y);
            OnResize += size => view.Size = new Vector2D<int>(size.Width, size.Height);
            
            var fb = new OpenGLRenderer(win);
            var imguiController = new ImGuiController(fb._gl, view, win.CreateInput());

            win.Update += delta => {
                imguiController.Update((float)delta);
            };

            win.Render += delta => {
                fb.Render(() => {
                    RequestRender(delta);
                    imguiController.Render();
                });
            };

        }, new Vector2D<int>(Size.Width, Size.Height), DisplayName);
    }
    
}