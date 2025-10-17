using System.Drawing;
using System.Numerics;
using DearOVRlay;
using ImGuiNET;
using Valve.VR;

namespace OpenVROverlayTakeTwo;

public class Program
{
    public static void Main(string[] args) {
        var app = new OverlayApp("test") {
            DisplayName = "C# + OpenVR Overlay + DearImGUI",
            ThumbnailImagePath = "C:\\Users\\foxt\\Downloads\\sunglass.png",
            WidthInMeters = 3.14f,
            Size = new Size(1920, 1080)
        };
        app.OnEvent += evt => {
            var name = evt.eventType.ToString();
            OVRUtils.OVREventTypes.TryGetValue(evt.eventType, out name);
            Console.WriteLine("Got event " + name);
        };
        app.OnRender += delta => {

            
            ImGui.SetNextWindowSize(new Vector2(app.Size.Width, app.Size.Height));
            ImGui.SetNextWindowPos(Vector2.Zero);
            ImGui.Begin("OpenVR Demo", ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoResize);
            for (uint i = 0; i < OpenVR.k_unMaxTrackedDeviceCount; i++) {
                var classType = OpenVR.System.GetTrackedDeviceClass(i);
                if (classType == ETrackedDeviceClass.Invalid) continue;
                var className = classType.ToString();
                var deviceModel =
                    OVRUtils.GetTrackedDeviceStringProperty(i, ETrackedDeviceProperty.Prop_ModelNumber_String);
                var deviceMfg =
                    OVRUtils.GetTrackedDeviceStringProperty(i, ETrackedDeviceProperty.Prop_ManufacturerName_String);
                
                
                ImGui.Text($"#{i}: {className} ({deviceMfg} {deviceModel})");

                try {
                    var hasBattery = OVRUtils.GetTrackedDeviceBoolProperty(i,
                        ETrackedDeviceProperty.Prop_DeviceProvidesBatteryStatus_Bool);
                    if (hasBattery == true) {
                        var batLevel =
                            OVRUtils.GetTrackedDeviceFloatProperty(i,
                                ETrackedDeviceProperty.Prop_DeviceBatteryPercentage_Float);
                        ImGui.Text($"    Battery level: {batLevel}");
                    }
                } catch (Exception e) {
                    ImGui.Text($"    Error: {e.Message}");
                }

            }
            
            
            ImGui.End();
            ImGui.ShowDemoWindow();
            ImGui.ShowMetricsWindow();
        };
        app.Run();
    }

}