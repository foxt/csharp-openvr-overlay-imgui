using System;
using System.Collections.Generic;
using System.Numerics;
using Silk.NET.Input;
using Valve.VR;

namespace DearOVRlay;

public class OVRMouse : IMouse {
    public string Name => "OpenVR Laser Pointer";
    public int Index => 0;
    public bool IsConnected => true;

    private IReadOnlyList<MouseButton> _SupportedButtons = new[] { MouseButton.Left, MouseButton.Right, MouseButton.Middle };
    public IReadOnlyList<MouseButton> SupportedButtons => _SupportedButtons;
    public HashSet<MouseButton> ButtonState = new();
    public bool IsButtonPressed(MouseButton btn) => ButtonState.Contains(btn);
    
    private Vector2 _Position = Vector2.Zero;
    public Vector2 Position {
        get => _Position;
        set => _Position = value;
    }
    
    public float ScrollX = 0;
    public float ScrollY = 0;
    public IReadOnlyList<ScrollWheel> ScrollWheels {
        get {
            var wheels = new[] { new ScrollWheel(ScrollX, ScrollY) };
            ScrollX = 0;
            ScrollY = 0;
            return wheels;
        }
    }


    public ICursor Cursor { get; }
    public int DoubleClickTime { get; set; }
    public int DoubleClickRange { get; set; }
    public event Action<IMouse, MouseButton>? MouseDown;
    public event Action<IMouse, MouseButton>? MouseUp;
    public event Action<IMouse, MouseButton, Vector2>? Click;
    public event Action<IMouse, MouseButton, Vector2>? DoubleClick;
    public event Action<IMouse, Vector2>? MouseMove;
    public event Action<IMouse, ScrollWheel>? Scroll;
    
}

public class OVRKeyboard : IKeyboard {
    public string Name { get; }
    public int Index { get; }
    public bool IsConnected { get; }
    public bool IsKeyPressed(Key key) {
        return false;
    }
    public bool IsScancodePressed(int scancode) {
        throw new NotImplementedException();
    }
    public void BeginInput() {
        throw new NotImplementedException();
    }
    public void EndInput() {
        throw new NotImplementedException();
    }
    public IReadOnlyList<Key> SupportedKeys { get; }
    public string ClipboardText { get; set; }
    public event Action<IKeyboard, Key, int>? KeyDown;
    public event Action<IKeyboard, Key, int>? KeyUp;
    public event Action<IKeyboard, char>? KeyChar;
}

public class OVRInputContext : IInputContext {
    private readonly IReadOnlyList<OVRKeyboard> _Keyboards = new[] { new OVRKeyboard() };
    public IReadOnlyList<IKeyboard> Keyboards => _Keyboards;
    private readonly IReadOnlyList<OVRMouse> _Mice = new[] { new OVRMouse() };
    public IReadOnlyList<IMouse> Mice => _Mice;

    public bool HandleOVREvent(VREvent_t evt) {
        var type = (EVREventType)evt.eventType;
        if (type == EVREventType.VREvent_MouseMove) {
            _Mice[0].Position = new Vector2(evt.data.mouse.x, evt.data.mouse.y);
        } else if (type == EVREventType.VREvent_MouseButtonDown || type == EVREventType.VREvent_MouseButtonUp) {
            var ovrButton = (EVRMouseButton)evt.data.mouse.button;
            var silkButton =
                ovrButton == EVRMouseButton.Middle ? Silk.NET.Input.MouseButton.Middle :
                ovrButton == EVRMouseButton.Right ? Silk.NET.Input.MouseButton.Right :
                Silk.NET.Input.MouseButton.Left;
            var isDown = type == EVREventType.VREvent_MouseButtonDown;
            
            if (isDown) _Mice[0].ButtonState.Add(silkButton);
            else _Mice[0].ButtonState.Remove(silkButton);
        }
        else if (type == EVREventType.VREvent_ScrollSmooth) {
            _Mice[0].ScrollX += evt.data.scroll.xdelta;
            _Mice[0].ScrollY += evt.data.scroll.ydelta;
        } else {
            return false;
        }

        return true;
    }
    
    // unused:
    public void Dispose() {
        throw new NotImplementedException();
    }
    public IntPtr Handle { get; }
    public IReadOnlyList<IGamepad> Gamepads { get; }
    public IReadOnlyList<IJoystick> Joysticks { get; }
    public IReadOnlyList<IInputDevice> OtherDevices { get; }
    public event Action<IInputDevice, bool>? ConnectionChanged;
}