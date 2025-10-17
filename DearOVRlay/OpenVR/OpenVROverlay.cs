using System;
using System.Diagnostics;
using Silk.NET.Maths;
using Valve.VR;

namespace DearOVRlay;

public class OpenVROverlay {
    private static CVRSystem _ovr;

    public static void InitOpenVR() {
        if (_ovr == null) {
            EVRInitError err = EVRInitError.None;
            _ovr = OpenVR.Init(ref err, EVRApplicationType.VRApplication_Overlay);
            if (err != EVRInitError.None) throw new Exception(err.ToString());
        }
    }
    public ulong _overlay;
    public ulong _overlayThumb;

    public OpenVROverlay(
        string id,
        string displayName
    ) {
        InitOpenVR();
        OVRUtils.CheckError(OpenVR.Overlay.CreateDashboardOverlay(id, displayName, ref _overlay, ref _overlayThumb));
        OVRUtils.CheckError(OpenVR.Overlay.SetOverlayFlag(_overlay, VROverlayFlags.EnableClickStabilization, true));
        OVRUtils.CheckError(OpenVR.Overlay.SetOverlayFlag(_overlay, VROverlayFlags.SendVRSmoothScrollEvents, true));
    }

    public string ThumbnailImagePath {
        set => OVRUtils.CheckError(OpenVR.Overlay.SetOverlayFromFile(_overlayThumb, value ));
    }
    public float WidthInMeters {
        set => OVRUtils.CheckError(OpenVR.Overlay.SetOverlayWidthInMeters(_overlay, value ));
    }

    private HmdVector2_t _hmdVector2;
    public Vector2D<int> OverlayMouseScale {
        set {
            _hmdVector2.v0 = value[0];
            _hmdVector2.v1 = value[1];
            OVRUtils.CheckError(OpenVR.Overlay.SetOverlayMouseScale(_overlay, ref _hmdVector2));
        }
    }

    public event Action<double> OnUpdate;
    public event Action<double> OnRender;
    public void Run() {
        var time = Stopwatch.StartNew();
        var last = time.Elapsed.TotalSeconds;
        while (true)  {
            var now = time.Elapsed.TotalSeconds;
            var delta = now - last;
            last = now;
            OnUpdate?.Invoke(delta);
            if (OpenVR.Overlay.IsOverlayVisible(_overlay)) OnRender?.Invoke(delta);
            OpenVR.Overlay.WaitFrameSync(100);
        }
    }
    
    private VREvent_t evt;
    private uint evtSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(VREvent_t));
    public event Action<VREvent_t> OnEvent; 
    
    public void PollEvents() {
        while (OpenVR.Overlay.PollNextOverlayEvent(_overlay, ref evt, evtSize)) {
            OnEvent?.Invoke(evt);
        }
    }
    
    
    private Texture_t _ovrTexture = new () {
        eColorSpace = EColorSpace.Auto,
        eType = ETextureType.OpenGL,
    };

    public void SubmitOpenGLTextureFrame(IntPtr gluint) {
        _ovrTexture.handle = (IntPtr)gluint;
        OVRUtils.CheckError(OpenVR.Overlay.SetOverlayTexture(_overlay, ref _ovrTexture));
    }

}