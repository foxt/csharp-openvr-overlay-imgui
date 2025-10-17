using System;
using System.Drawing;
using System.Linq;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL.Extensions.ImGui;
using Valve.VR;

namespace DearOVRlay;

public class OverlayApp {
    private OVRInputContext input = new();
    private FakeView view;
    public readonly OpenVROverlay Overlay;

    public event Action<Size> OnResize;
    private Size _size = Size.Empty;
    public Size Size {
        get => _size;
        set {
            if (_size.Equals(value)) return;
            _size = value;
            OnResize?.Invoke(value);
        }
    }

    public string ThumbnailImagePath {
        set => Overlay.ThumbnailImagePath = value;
    }

    public float WidthInMeters {
        set => Overlay.WidthInMeters = value;
    }

    private string _displayName = "OverlayApp";

    public string DisplayName {
        get => _displayName;
        set {
            _displayName = value;
            if (Overlay != null) Overlay.DisplayName = value;
        }
    }

    public event Action<VREvent_t> OnEvent;
    public event Action<double> OnRender;

    private bool _isKeyboardVisible = false;

    private bool KeyboardVisible {
        get => _isKeyboardVisible;
        set {
            if (value == _isKeyboardVisible) return;
            if (value) {
                OVRUtils.CheckError(OpenVR.Overlay.ShowKeyboardForOverlay(
                    Overlay._overlay,
                    (int)EGamepadTextInputMode.k_EGamepadTextInputModeNormal,
                    (int)EGamepadTextInputLineMode.k_EGamepadTextInputLineModeMultipleLines,
                    (int)EKeyboardFlags.KeyboardFlag_Minimal | (int)EKeyboardFlags.KeyboardFlag_ShowArrowKeys,
                    "ImGui Keyboard",
                    uint.MaxValue,
                    "",
                    0
                ));
            } else OpenVR.Overlay.HideKeyboard();
        }
    }
    
    public OverlayApp() {}

    public OverlayApp(string id) {
        view = new FakeView(Size.Width, Size.Height);
        view.Resize += vec => this.Size = new Size(vec.X, vec.Y);
        OnResize += size => view.Size = new Vector2D<int>(size.Width, size.Height);

        Overlay = new(id, DisplayName) { OverlayMouseScale = Size };
        OnResize += size => Overlay.OverlayMouseScale = size;
        Overlay.OnEvent += evt => {
            var imguiIO = ImGui.GetIO();
            var type = (EVREventType)evt.eventType;
            if (type == EVREventType.VREvent_KeyboardOpened_Global) {
                _isKeyboardVisible = true;
            }
            else if (type == EVREventType.VREvent_KeyboardClosed_Global) {
                _isKeyboardVisible = false;
            } else if (type == EVREventType.VREvent_KeyboardCharInput) {
                
                var dat = evt.data.keyboard;
                var buf = new[] {
                    dat.cNewInput0,
                    dat.cNewInput1,
                    dat.cNewInput2,
                    dat.cNewInput3,
                    dat.cNewInput4,
                    dat.cNewInput5,
                    dat.cNewInput6,
                    dat.cNewInput7,
                };
                ImGuiKey? key = null;
                Console.WriteLine($"    Got input: {String.Join(' ', buf.Select(a => ((int)a).ToString()))}");
                // assuming 1 input event per key (at least for specials like backsp, arrows, etc)
                if (buf[0] == 8 && buf[1] == 0) key = ImGuiKey.Backspace;
                else if (buf[0] == 9 && buf[1] == 0) key = ImGuiKey.Tab;
                else if (buf[0] == 10 && buf[1] == 0) key = ImGuiKey.Enter;
                else if (buf[0] == 27 && buf[1] == 91 && buf[2] == 65 && buf[3] == 0) key = ImGuiKey.UpArrow;
                else if (buf[0] == 27 && buf[1] == 91 && buf[2] == 66 && buf[3] == 0) key = ImGuiKey.DownArrow;
                else if (buf[0] == 27 && buf[1] == 91 && buf[2] == 67 && buf[3] == 0) key = ImGuiKey.RightArrow;
                else if (buf[0] == 27 && buf[1] == 91 && buf[2] == 68 && buf[3] == 0) key = ImGuiKey.LeftArrow;

                if (key is not null) {
                    imguiIO.AddKeyEvent((ImGuiKey)key, true);
                    imguiIO.AddKeyEvent((ImGuiKey)key, false);
                }
                else {
                    // send as text
                    var text = System.Text.Encoding.UTF8.GetString(buf);

                    imguiIO.AddInputCharactersUTF8(text);
                }

            } else if (type == EVREventType.VREvent_KeyboardDone) {
                ImGui.SetWindowFocus();
            } else if (!input.HandleOVREvent(this, evt)) {
                OnEvent?.Invoke(evt);
            }
        };
    }

    protected void RequestRender(double delta) {
        OnRender?.Invoke(delta);
    }
    public void Run() {
        OpenGLRenderer.InitOffscreen(gl => {
            var fb = new OpenGLRenderer(gl, view.Size);
            view.Resize += size => fb.Size = size;
            
            var imguiController = new ImGuiController(fb._gl, view, input);
            var imguiIO = ImGui.GetIO();
            var wasActive = false;
            
            
            Overlay.OnUpdate += delta => {
                Overlay.PollEvents();

                var isActive = OpenVR.Overlay.IsActiveDashboardOverlay(Overlay._overlay);
                var isRequestingKeyboard =
                    imguiIO.WantTextInput && isActive;
                // send an event one last time after becoming deactivated so we actually close the keyboard
                // (steamvr _does_ auto close the keyboard when becoming deactivated,
                // but it does this a few frames before we actually get told we're deactive,
                // so without this check, we'd just pop the keyboard back up which isnt great)
                if (isActive || wasActive) {
                    KeyboardVisible = isRequestingKeyboard;
                    wasActive = isActive;
                }
                
                imguiController.Update((float)delta);
            };

            Overlay.OnRender += delta => {
                fb.Render(() => {
                    RequestRender(delta);
                    imguiController.Render();
                });
                Overlay.SubmitOpenGLTextureFrame((IntPtr)fb.texture);
            };

            Overlay.Run();
        });
    }
    
}