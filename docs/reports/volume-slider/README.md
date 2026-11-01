# Volume slider checks

The settings panel now uses a continuous 0–100% Slider. Pointer input updates audio and percentage immediately; releasing the control, changing focus or closing the panel saves pending changes without writing on every drag frame.

The current real-frame suite checks actual slider pointer/drag input, audio source volumes, 0% mute, release-to-save, close-to-save and restored position. The legacy test project passes 18 checks including 5 volume checks and its existing completion-notice checks. Both suites report 0 runtime errors. Mac/Windows local builds report 0 errors and 0 warnings. Windows remains cross-built without native hardware verification. Public GitHub release assets are unchanged.
