using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.VR;

namespace DearOVRlay
{
	public class OVRUtils
    {
        public static Dictionary<uint, string> OVREventTypes = new() {
			{ 0, "None" },
			{ 100, "TrackedDeviceActivated" },
			{ 101, "TrackedDeviceDeactivated" },
			{ 102, "TrackedDeviceUpdated" },
			{ 103, "TrackedDeviceUserInteractionStarted" },
			{ 104, "TrackedDeviceUserInteractionEnded" },
			{ 105, "IpdChanged" },
			{ 106, "EnterStandbyMode" },
			{ 107, "LeaveStandbyMode" },
			{ 108, "TrackedDeviceRoleChanged" },
			{ 109, "WatchdogWakeUpRequested" },
			{ 110, "LensDistortionChanged" },
			{ 111, "PropertyChanged" },
			{ 112, "WirelessDisconnect" },
			{ 113, "WirelessReconnect" },
			{ 114, "Reserved_0114" },
			{ 115, "Reserved_0115" },
			{ 200, "ButtonPress" }, // data is controller
			{ 201, "ButtonUnpress" }, // data is controller
			{ 202, "ButtonTouch" }, // data is controller
			{ 203, "ButtonUntouch" }, // data is controller
			// { 250, "DualAnalog_Press" }, // No longer sent
			// { 251, "DualAnalog_Unpress" }, // No longer sent
			// { 252, "DualAnalog_Touch" }, // No longer sent
			// { 253, "DualAnalog_Untouch" }, // No longer sent
			// { 254, "DualAnalog_Move" }, // No longer sent
			// { 255, "DualAnalog_ModeSwitch1" }, // No longer sent
			// { 256, "DualAnalog_ModeSwitch2" }, // No longer sent
			{ 257, "Modal_Cancel" }, // Sent to overlays with the
			{ 300, "MouseMove" }, // data is mouse
			{ 301, "MouseButtonDown" }, // data is mouse
			{ 302, "MouseButtonUp" }, // data is mouse
			{ 303, "FocusEnter" }, // data is overlay
			{ 304, "FocusLeave" }, // data is overlay
			{ 305, "ScrollDiscrete" }, // data is scroll
			{ 306, "TouchPadMove" }, // data is mouse
			{ 307, "OverlayFocusChanged" }, // data is overlay, global event
			{ 308, "ReloadOverlays" },
			{ 309, "ScrollSmooth" }, // data is scroll
			{ 310, "LockMousePosition" }, // data is mouse
			{ 311, "UnlockMousePosition" }, // data is mouse
			{ 400, "InputFocusCaptured" }, // data is process DEPRECATED
			{ 401, "InputFocusReleased" }, // data is process DEPRECATED
			// { 402, "SceneFocusLost" }, // data is process
			// { 403, "SceneFocusGained" }, // data is process
			{ 404, "SceneApplicationChanged" }, // data is process - The App actually drawing the scene changed (usually to or from the compositor)
			// { 405, "SceneFocusChanged" }, // data is process - This is defunct and will not be called.
			{ 406, "InputFocusChanged" }, // data is process
			// { 407, "SceneApplicationSecondaryRenderingStarted" },
			{ 408, "SceneApplicationUsingWrongGraphicsAdapter" }, // data is process
			{ 409, "ActionBindingReloaded" }, // data is process - The App that action binds reloaded for
			{ 410, "HideRenderModels" }, // Sent to the scene application to request hiding render models temporarily
			{ 411, "ShowRenderModels" }, // Sent to the scene application to request restoring render model visibility
			{ 412, "SceneApplicationStateChanged" }, // No data; but query VRApplications()->GetSceneApplicationState();
			{ 413, "SceneAppPipeDisconnected" }, // data is process - Called when the scene app's pipe has been closed.
			{ 420, "ConsoleOpened" },
			{ 421, "ConsoleClosed" },
			{ 500, "OverlayShown" }, // Indicates that an overlay is now visible to someone and should be rendering normally. Reflects IVROverlay::IsOverlayVisible() becoming true.
			{ 501, "OverlayHidden" }, // Indicates that an overlay is no longer visible to someone and doesn't need to render frames. Reflects IVROverlay::IsOverlayVisible() becoming false.
			{ 502, "DashboardActivated" },
			{ 503, "DashboardDeactivated" },
			//{ 504, "DashboardThumbSelected" }, // Sent to the overlay manager - data is overlay - No longer sent
			//{ 505, "DashboardRequested" }, // Sent to the overlay manager - data is overlay
			{ 506, "ResetDashboard" }, // Send to the overlay manager
			//{ 507, "RenderToast" }, // Send to the dashboard to render a toast - data is the notification ID -- no longer sent
			{ 508, "ImageLoaded" }, // Sent to overlays when a SetOverlayRaw or SetOverlayFromFile call finishes loading
			{ 509, "ShowKeyboard" }, // Sent to keyboard renderer in the dashboard to invoke it
			{ 510, "HideKeyboard" }, // Sent to keyboard renderer in the dashboard to hide it
			{ 511, "OverlayGamepadFocusGained" }, // Sent to an overlay when IVROverlay::SetFocusOverlay is called on it
			{ 512, "OverlayGamepadFocusLost" }, // Send to an overlay when it previously had focus and IVROverlay::SetFocusOverlay is called on something else
			{ 513, "OverlaySharedTextureChanged" },
			//{ 514, "DashboardGuideButtonDown" }, // These are no longer sent
			//{ 515, "DashboardGuideButtonUp" },
			{ 516, "ScreenshotTriggered" }, // Screenshot button combo was pressed, Dashboard should request a screenshot
			{ 517, "ImageFailed" }, // Sent to overlays when a SetOverlayRaw or SetOverlayfromFail fails to load
			{ 518, "DashboardOverlayCreated" },
			{ 519, "SwitchGamepadFocus" },
			// Screenshot API
			{ 520, "RequestScreenshot" }, // Sent by vrclient application to compositor to take a screenshot
			{ 521, "ScreenshotTaken" }, // Sent by compositor to the application that the screenshot has been taken
			{ 522, "ScreenshotFailed" }, // Sent by compositor to the application that the screenshot failed to be taken
			{ 523, "SubmitScreenshotToDashboard" }, // Sent by compositor to the dashboard that a completed screenshot was submitted
			{ 524, "ScreenshotProgressToDashboard" }, // Sent by compositor to the dashboard that a completed screenshot was submitted
			{ 525, "PrimaryDashboardDeviceChanged" },
			{ 526, "RoomViewShown" }, // Sent by compositor whenever room-view is enabled
			{ 527, "RoomViewHidden" }, // Sent by compositor whenever room-view is disabled
			{ 528, "ShowUI" }, // data is showUi
			{ 529, "ShowDevTools" }, // data is showDevTools
			{ 530, "DesktopViewUpdating" },
			{ 531, "DesktopViewReady" },
			{ 532, "StartDashboard" },
			{ 533, "ElevatePrism" },
			{ 534, "OverlayClosed" },
			{ 535, "DashboardThumbChanged" }, // Sent when a dashboard thumbnail image changes
			{ 536, "DesktopMightBeVisible" }, // Sent when any known desktop related overlay is visible
			{ 537, "DesktopMightBeHidden" }, // Sent when all known desktop related overlays are hidden
			{ 538, "MutualSteamCapabilitiesChanged" }, // Sent when the set of capabilities common between both Steam and SteamVR have changed.
			{ 539, "OverlayCreated" }, // An OpenVR overlay of any sort was created. Data is overlay.
			{ 540, "OverlayDestroyed" }, // An OpenVR overlay of any sort was destroyed. Data is overlay.
			{ 541, "TrackingRecordingStarted" },
			{ 542, "TrackingRecordingStopped" },
			{ 543, "SetTrackingRecordingPath" },
			{ 560, "Reserved_0560" }, // No data
			{ 561, "Reserved_0561" }, // No data
			{ 562, "Reserved_0562" }, // No data
			{ 563, "Reserved_0563" }, // No data
			{ 600, "Notification_Shown" },
			{ 601, "Notification_Hidden" },
			{ 602, "Notification_BeginInteraction" },
			{ 603, "Notification_Destroyed" },
			{ 700, "Quit" }, // data is process
			{ 701, "ProcessQuit" }, // data is process
			//{ 702, "QuitAborted_UserPrompt" }, // data is process
			{ 703, "QuitAcknowledged" }, // data is process
			{ 704, "DriverRequestedQuit" }, // The driver has requested that SteamVR shut down
			{ 705, "RestartRequested" }, // A driver or other component wants the user to restart SteamVR
			{ 706, "InvalidateSwapTextureSets" },
			{ 707, "RequestDisconnectWirelessHMD" }, // vrserver asks vrlink to disconnect
			{ 800, "ChaperoneDataHasChanged" }, // this will never happen with the new chaperone system
			{ 801, "ChaperoneUniverseHasChanged" },
			{ 802, "ChaperoneTempDataHasChanged" }, // this will never happen with the new chaperone system
			{ 803, "ChaperoneSettingsHaveChanged" },
			{ 804, "SeatedZeroPoseReset" },
			{ 805, "ChaperoneFlushCache" }, // Sent when the process needs to reload any cached data it retrieved from VRChaperone()
			{ 806, "ChaperoneRoomSetupStarting" }, // Triggered by CVRChaperoneClient::RoomSetupStarting
			{ 807, "ChaperoneRoomSetupCommitted" }, // Triggered by CVRChaperoneClient::CommitWorkingCopy (formerly VREvent_ChaperoneRoomSetupFinished)
			{ 808, "StandingZeroPoseReset" },
			{ 809, "Reserved_0809" },
			{ 810, "Reserved_0810" },
			{ 811, "Reserved_0811" },
			{ 820, "AudioSettingsHaveChanged" },
			{ 850, "BackgroundSettingHasChanged" },
			{ 851, "CameraSettingsHaveChanged" },
			{ 852, "ReprojectionSettingHasChanged" },
			{ 853, "ModelSkinSettingsHaveChanged" },
			{ 854, "EnvironmentSettingsHaveChanged" },
			{ 855, "PowerSettingsHaveChanged" },
			{ 856, "EnableHomeAppSettingsHaveChanged" },
			{ 857, "SteamVRSectionSettingChanged" },
			{ 858, "LighthouseSectionSettingChanged" },
			{ 859, "NullSectionSettingChanged" },
			{ 860, "UserInterfaceSectionSettingChanged" },
			{ 861, "NotificationsSectionSettingChanged" },
			{ 862, "KeyboardSectionSettingChanged" },
			{ 863, "PerfSectionSettingChanged" },
			{ 864, "DashboardSectionSettingChanged" },
			{ 865, "WebInterfaceSectionSettingChanged" },
			{ 866, "TrackersSectionSettingChanged" },
			{ 867, "LastKnownSectionSettingChanged" },
			{ 868, "DismissedWarningsSectionSettingChanged" },
			{ 869, "GpuSpeedSectionSettingChanged" },
			{ 870, "WindowsMRSectionSettingChanged" },
			{ 871, "OtherSectionSettingChanged" },
			{ 872, "AnyDriverSettingsChanged" },
			{ 900, "StatusUpdate" },
			{ 950, "WebInterface_InstallDriverCompleted" },
			{ 1000, "MCImageUpdated" },
			{ 1100, "FirmwareUpdateStarted" },
			{ 1101, "FirmwareUpdateFinished" },
			{ 1200, "KeyboardClosed" }, // DEPRECATED: Sent only to the overlay it closed for, or globally if it was closed for a scene app
			{ 1201, "KeyboardCharInput" }, // Sent on keyboard input. Warning: event type appears as both global event and overlay event
			{ 1202, "KeyboardDone" }, // Sent when DONE button clicked on keyboard. Warning: event type appears as both global event and overlay event
			{ 1203, "KeyboardOpened_Global" }, // Sent globally when the keyboard is opened. data.keyboard.overlayHandle is who it was opened for (scene app if k_ulOverlayHandleInvalid)
			{ 1204, "KeyboardClosed_Global" }, // Sent globally when the keyboard is closed. data.keyboard.overlayHandle is who it was opened for (scene app if k_ulOverlayHandleInvalid)
			//{ 1300, "ApplicationTransitionStarted" },
			//{ 1301, "ApplicationTransitionAborted" },
			//{ 1302, "ApplicationTransitionNewAppStarted" },
			{ 1303, "ApplicationListUpdated" },
			{ 1304, "ApplicationMimeTypeLoad" },
			// { 1305, "ApplicationTransitionNewAppLaunchComplete" },
			{ 1306, "ProcessConnected" },
			{ 1307, "ProcessDisconnected" },
			//{ 1400, "Compositor_MirrorWindowShown" }, // DEPRECATED
			//{ 1401, "Compositor_MirrorWindowHidden" }, // DEPRECATED
			{ 1410, "Compositor_ChaperoneBoundsShown" },
			{ 1411, "Compositor_ChaperoneBoundsHidden" },
			{ 1412, "Compositor_DisplayDisconnected" },
			{ 1413, "Compositor_DisplayReconnected" },
			{ 1414, "Compositor_HDCPError" }, // data is hdcpError
			{ 1415, "Compositor_ApplicationNotResponding" },
			{ 1416, "Compositor_ApplicationResumed" },
			{ 1417, "Compositor_OutOfVideoMemory" },
			{ 1418, "Compositor_DisplayModeNotSupported" }, // k_pch_SteamVR_PreferredRefreshRate
			{ 1419, "Compositor_StageOverrideReady" },
			{ 1420, "Compositor_RequestDisconnectReconnect" },
			{ 1500, "TrackedCamera_StartVideoStream" },
			{ 1501, "TrackedCamera_StopVideoStream" },
			{ 1502, "TrackedCamera_PauseVideoStream" },
			{ 1503, "TrackedCamera_ResumeVideoStream" },
			{ 1550, "TrackedCamera_EditingSurface" },
			{ 1600, "PerformanceTest_EnableCapture" },
			{ 1601, "PerformanceTest_DisableCapture" },
			{ 1602, "PerformanceTest_FidelityLevel" },
			{ 1650, "MessageOverlay_Closed" },
			{ 1651, "MessageOverlayCloseRequested" },
			{ 1700, "Input_HapticVibration" }, // data is hapticVibration
			{ 1701, "Input_BindingLoadFailed" }, // data is inputBinding
			{ 1702, "Input_BindingLoadSuccessful" }, // data is inputBinding
			{ 1703, "Input_ActionManifestReloaded" }, // no data
			{ 1704, "Input_ActionManifestLoadFailed" }, // data is actionManifest
			{ 1705, "Input_ProgressUpdate" }, // data is progressUpdate
			{ 1706, "Input_TrackerActivated" },
			{ 1707, "Input_BindingsUpdated" },
			{ 1708, "Input_BindingSubscriptionChanged" },
			{ 1800, "SpatialAnchors_PoseUpdated" },        // data is spatialAnchor. broadcast
			{ 1801, "SpatialAnchors_DescriptorUpdated" },       // data is spatialAnchor. broadcast
			{ 1802, "SpatialAnchors_RequestPoseUpdate" },       // data is spatialAnchor. sent to specific driver
			{ 1803, "SpatialAnchors_RequestDescriptorUpdate" }, // data is spatialAnchor. sent to specific driver
			{ 1900, "SystemReport_Started" }, // user or system initiated generation of a system report. broadcast
			{ 2000, "Monitor_ShowHeadsetView" }, // data is process
			{ 2001, "Monitor_HideHeadsetView" }, // data is process
			{ 2100, "Audio_SetSpeakersVolume" },
			{ 2101, "Audio_SetSpeakersMute" },
			{ 2102, "Audio_SetMicrophoneVolume" },
			{ 2103, "Audio_SetMicrophoneMute" },
			{ 2200, "RenderModel_CountChanged" }, //Number of RenderModels in the system has changed
			// Vendors are free to expose private events in this reserved region
			{ 10000, "VendorSpecific_Reserved_Start" },
			{ 19999, "VendorSpecific_Reserved_End" },
			};

		public static void CheckError(EVROverlayError error)
		{
			if (error != EVROverlayError.None)
				throw new Exception(error.ToString());
		}

		private static StringBuilder stringBuilder = new(64);
		private static ETrackedPropertyError _eTrackedPropertyError = ETrackedPropertyError.TrackedProp_Success;

		private static T? TrackedPropertyErrorCheck<T>(Func<T> call) {
			_eTrackedPropertyError = ETrackedPropertyError.TrackedProp_Success;
			var value = call();
			if (_eTrackedPropertyError == ETrackedPropertyError.TrackedProp_Success)
				return value;
			else if (_eTrackedPropertyError == ETrackedPropertyError.TrackedProp_UnknownProperty)
				return default;
			else
				throw new Exception(_eTrackedPropertyError.ToString());
			
		}
		public static string? GetTrackedDeviceStringProperty(uint deviceIdx, ETrackedDeviceProperty property) {
			try {
				
				var ret = TrackedPropertyErrorCheck(() => OpenVR.System.GetStringTrackedDeviceProperty(deviceIdx, property,
					stringBuilder, (uint)stringBuilder.Capacity, ref _eTrackedPropertyError));
				if (ret == null) return null;
				return stringBuilder.ToString();
			} finally { stringBuilder.Clear(); }
		}
		public static bool? GetTrackedDeviceBoolProperty(uint deviceIdx, ETrackedDeviceProperty property) {
			return TrackedPropertyErrorCheck(() =>
				OpenVR.System.GetBoolTrackedDeviceProperty(deviceIdx, property, ref _eTrackedPropertyError));
		}
		public static float? GetTrackedDeviceFloatProperty(uint deviceIdx, ETrackedDeviceProperty property) {
			return TrackedPropertyErrorCheck(() =>
				OpenVR.System.GetFloatTrackedDeviceProperty(deviceIdx, property, ref _eTrackedPropertyError));
		}

		public static int? GetTrackedDeviceInt32Property(uint deviceIdx, ETrackedDeviceProperty property) {
			return TrackedPropertyErrorCheck(() =>
				OpenVR.System.GetInt32TrackedDeviceProperty(deviceIdx, property, ref _eTrackedPropertyError));
		}
		public static ulong? GetTrackedDeviceUint64Property(uint deviceIdx, ETrackedDeviceProperty property) {
			return TrackedPropertyErrorCheck(() =>
				OpenVR.System.GetUint64TrackedDeviceProperty(deviceIdx, property, ref _eTrackedPropertyError));
		}

		public static object GetTrackedDeviceProperty(uint deviceIdx, ETrackedDeviceProperty property) {
			var type = property.ToString().Split('_').Last();
			if (type == "String") return GetTrackedDeviceStringProperty(deviceIdx, property);
			else if (type == "Bool") return GetTrackedDeviceBoolProperty(deviceIdx, property);
			else if (type == "Float") return GetTrackedDeviceFloatProperty(deviceIdx, property);
			else if (type == "Int32") return GetTrackedDeviceInt32Property(deviceIdx, property);
			else if (type == "Uint64") return GetTrackedDeviceUint64Property(deviceIdx, property);
			else return $"(unknown type {type})";
		}
    }
}
