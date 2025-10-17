# C# Overlay for OpenVR using DearImGui

## Features

 - [x] Render ImGui to OpenVR overlay
   - [X] Vsync with headset display
   - [X] Pause rendering when overlay hidden
 - [X] Render to window (for debugging)
   - [ ] Hotswitch between windowed/OpenVR mode
 - [x] Pass inputs from OpenVR to ImGui
   - [x] Mouse move
   - [x] Left, right & middle click
   - [x] Smooth scrolling
   - [x] Keyboard support
     - [x] Automatically hide show keyboard

## Known Issues
 - [X] ~~Mouse input is flipped vertically when using the overlay viewer~~

## Requirements

- Download [`openvr_api.cs`](https://raw.githubusercontent.com/ValveSoftware/openvr/refs/heads/master/headers/openvr_api.cs) & place in `DearOVRlay\openvr_api\openvr_api.cs`
- Download [`openvr_api.dll`](https://github.com/ValveSoftware/openvr/raw/refs/heads/master/bin/win64/openvr_api.dll) (win64) & place in `DearOVRlay\openvr_api\openvr_api.dll`

