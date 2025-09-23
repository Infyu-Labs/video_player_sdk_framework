
# 🎮 Video Player SDK for Unity (iOS)

This SDK bridges native Swift-based video playback functionality with Unity using P/Invoke, offering a seamless experience for playing, seeking, and controlling video content in your Unity applications.

## 📦 Installation

- Clone the repo or drag the framework into your Xcode Unity project.
- Ensure the `.framework` is embedded & signed in the iOS target's **Frameworks, Libraries, and Embedded Content** section.
- Enable Objective-C and C++ support if needed in Unity's iOS Build Settings.
- Link the framework using Unity’s `Plugins/iOS` folder structure.
- **Set the minimum deployment target to iOS 15.0** for the framework.

## 🎯 Key Features

- ✅ Supports video formats: HLS, MP4, MOV
- ✅ Playback controls:
  - Play / Pause / Resume / Stop
  - Seek Forward / Backward
  - Skip +10 / -10 seconds
  - Scrub slider
  - Next / Previous video
- ✅ Auto-hide controls
- ✅ Exit/Back button support
- ✅ Swift → Unity callback on control events

---

## 🔁 Swift → Unity Callbacks

All video actions now automatically notify Unity via the `sendUnityMessage` helper function.  
Messages are sent to the `GameManager` GameObject with method names corresponding to the action.

| Action           | Unity Method           | Message Example     |
|-----------------|----------------------|------------------|
| Play video       | OnVideoPlay           | "Started"        |
| Pause video      | OnVideoPause          | "Paused"         |
| Stop video       | OnVideoStop           | "Stopped"        |
| Seek forward     | OnVideoSeekForward    | "10.0"           |
| Seek backward    | OnVideoSeekBackward   | "10.0"           |
| Seek to specific time | OnVideoSeekTo     | "120.0"          |
| Back / Close     | OnVideoClosed         | "User exited"    |
| Video finished   | OnVideoFinished       | "Completed"      |

### Example Unity `SkidosVideoPlayer` Script

```csharp
using UnityEngine;

public class SkidosVideoPlayer : MonoBehaviour
{
    public void OnVideoPlay(string msg) {
        Debug.Log("Unity received play event: " + msg);
        AudioListener.pause = false; // Resume game sound
    }

    public void OnVideoPause(string msg) {
        Debug.Log("Unity received pause event: " + msg);
    }

    public void OnVideoStop(string msg) {
        Debug.Log("Unity received stop event: " + msg);
        AudioListener.pause = false;
    }

    public void OnVideoSeekForward(string msg) {
        Debug.Log("Unity received seek forward: " + msg);
    }

    public void OnVideoSeekBackward(string msg) {
        Debug.Log("Unity received seek backward: " + msg);
    }

    public void OnVideoSeekTo(string msg) {
        Debug.Log("Unity received seek to: " + msg);
    }

    public void OnVideoClosed(string msg) {
        Debug.Log("Unity received exit event: " + msg);
        AudioListener.pause = false;
    }

    public void OnVideoFinished(string msg) {
        Debug.Log("Unity received finished event: " + msg);
        AudioListener.pause = false;
    }
}


### 📘 Example Usage in Unity

To play a video using the SDK:

```csharp
using System.Runtime.InteropServices;
using UnityEngine;

public class VideoController : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void loadVideo(string url);

    [DllImport("__Internal")]
    private static extern void play();

    public void LoadAndPlay()
    {
      ##if UNITY_IOS && !UNITY_EDITOR
        loadVideo("https://example.com/video.m3u8");
        play();
      ##endif
    }
}


## 🧹 Unity Integration

1. Create a C# MonoBehaviour Script (if not using the one provided).

2. Use the `VideoPlayerBridge.cs` in your Unity project:


## Usage Guidelines

### Mandatory Initialization

Before calling `Play()`, you **must** call `SetURLs()` with an array of valid video URLs. Failing to do so may cause a native crash.

Example in Unity MonoBehaviour:

```csharp
public class VideoPlayerController : MonoBehaviour
{
    public string[] videoURLs;
    private VideoPlayerBridge playerBridge;

    void Start()
    {
        playerBridge = GetComponent<VideoPlayerBridge>();
        playerBridge.SetURLs(videoURLs);
        playerBridge.Play();
    }

    void OnApplicationQuit()
    {
        playerBridge.Cleanup();
    }
    
    void OnApplicationPause ( bool pause )
    {
        if ( !pause )    playerBridge.Play();
    }
}
```

### Cleaning Up

Always call `Cleanup()` in `OnApplicationQuit()` to release native resources and prevent background playback.

### UI Control Visibility

You can show or hide the built-in UI controls dynamically via the following methods on `VideoPlayerBridge`:

| Method                 | Description                          |
|------------------------|------------------------------------|
| ShowForwardButton(bool) | Show or hide the forward button    |
| ShowBackwordButton(bool) | Show or hide the backward button   |
| ShowBack10Button(bool)  | Show or hide the back 10 seconds button |
| ShowFor10Button(bool)   | Show or hide the forward 10 seconds button |
| ShowPlayPauseButton(bool) | Show or hide the play/pause button |
| ShowBackButton(bool)    | Show or hide the back button       |
| ShowLogo(bool)          | Show or hide the logo              |
| ShowSeekbar(bool)       | Show or hide the seek bar          |
| ShowTimeDuration(bool)  | Show or hide the time duration display |

Example:

```csharp
playerBridge.ShowPlayPauseButton(true);
playerBridge.ShowSeekbar(false);
```


## ⚠️ Usage Guidelines

- Set the video URLs using `SetURLs()` **before** calling `Play()`.
- Always call `Cleanup()` in `OnApplicationQuit()` to avoid background playback.
- Avoid calling `Play()` without setting URLs — it may result in a native crash.

## 📞 Example Swift Call (Optional)

If integrating on iOS directly, use:

```swift
Button("Play Video") {
    VideoPlayerService.shared.setURLS(urls: urls)
    VideoPlayerService.shared.play()
}
.onAppear {
    VideoPlayerService.shared.setShowFor10Button(true)
    VideoPlayerService.shared.setShowBack10Button(true)
}
```
### [1.0.0] - 2025-06-09
- Fixed layout issues in landscape on iPhone
- Controls now disappear after timeout
- Progress bar no longer changes thickness dynamically
- Timer panel stays consistent in size and position
- Resolved video freeze on resume from background

### [1.1.0] – 2025-06-12
- Fixed video freeze issue when resuming the app from the background.
- Made progress bar evenly shaped on both sides.
- Added proper spacing for the back button and progress bar in portrait mode.
- Improved playback stability when switching between background and foreground.
