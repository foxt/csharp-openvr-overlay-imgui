using DearOVRlay;
using ImGuiNET;

namespace OpenVROverlayTakeTwo;

public class Program
{
    public static void Main(string[] args) {
        var app = new OverlayApp(
            "test",
            "C# + OpenVR Overlay + DearImGUI",
            800,
            600
        ) {
            ThumbnailImagePath = "C:\\Users\\foxt\\Downloads\\sunglass.png",
            WidthInMeters = 3.14f
        };
        app.OnEvent += evt => {
            var name = evt.eventType.ToString();
            OVRUtils.OVREventTypes.TryGetValue(evt.eventType, out name);
            Console.WriteLine("Got event " + name);
        };
        app.OnRender += delta => {
            ImGui.ShowDemoWindow();
            ImGui.ShowMetricsWindow();
        };
        app.Run();
    }

}