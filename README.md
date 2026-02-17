# 🎮 Video Player SDK for Unity (iOS)

This SDK bridges native Swift-based video playback functionality with
Unity using P/Invoke, enabling seamless playlist-based video playback
inside Unity applications.

------------------------------------------------------------------------

# 📦 Installation

1.  Clone the repository or drag the `.framework` into your Xcode Unity
    project.
2.  Ensure the framework is **Embedded & Signed** in: **Target →
    Frameworks, Libraries & Embedded Content**
3.  Place the framework inside: Assets/Plugins/iOS
4.  Enable Objective-C support in Unity iOS Build Settings (if
    required).
5.  Set the minimum deployment target to **iOS 15.0**.

------------------------------------------------------------------------

# 🎯 Key Features

-   ✅ Supports video formats: HLS, MP4, MOV\
-   ✅ Playlist-based playback (v1.1.1+)\
-   ✅ Premium / Non-premium support\
-   ✅ Playback controls:
    -   Play / Pause / Stop
    -   Seek Forward / Backward
    -   Skip +10 / -10 seconds
    -   Scrub slider
    -   Next / Previous video
-   ✅ Auto-hide controls
-   ✅ Exit / Back button support
-   ✅ Swift → Unity callbacks

------------------------------------------------------------------------

# 🚀 IMPORTANT --- Playlist Flow (v1.1.1+)

Starting from **v1.1.1**, the SDK uses a **Playlist-first approach**.

## ✅ Mandatory Order

1️⃣ Call SetPlaylists(json)\
2️⃣ Call PlayPlaylist(playlistId)

You must call `SetPlaylists()` first before calling `PlayPlaylist()`.

------------------------------------------------------------------------

# 📘 Playlist JSON Format

``` json
[
  {
    "id": "playlist_1",
    "name": "Dino World",
    "thumbnail": "https://image.com/thumb.jpg",
    "description": "Kids learning series",
    "videos": [
      {
        "id": "vid1",
        "url": "https://video1.m3u8",
        "title": "Episode 1"
      }
    ]
  }
]
```

------------------------------------------------------------------------

# 💎 Premium Control

``` csharp
playerBridge.SetPremium(true);   // Premium user
playerBridge.SetPremium(false);  // Non-premium user
```

------------------------------------------------------------------------

# 🔁 Swift → Unity Callbacks

Messages are sent to the **SkidosVideoPlayer GameObject**.

  Action                  Unity Method          Message Example
  ----------------------- --------------------- -----------------
  Play video              OnVideoPlay           "Started"
  Pause video             OnVideoPause          "Paused"
  Stop video              OnVideoStop           "Stopped"
  Seek forward            OnVideoSeekForward    "10.0"
  Seek backward           OnVideoSeekBackward   "10.0"
  Seek to specific time   OnVideoSeekTo         "120.0"
  Back / Close            OnVideoClosed         "User exited"
  Video finished          OnVideoFinished       "Completed"

------------------------------------------------------------------------

# 🧹 Cleanup (Mandatory)

Always call:

``` csharp
playerBridge.Cleanup();
```

Inside:

``` csharp
void OnApplicationQuit()
```

------------------------------------------------------------------------

# 🔄 Migration Note

Old Flow: SetURLs() → Play()

New Flow (v1.1.1+): SetPlaylists() → PlayPlaylist()

------------------------------------------------------------------------

# 📝 Version History

## \[1.1.1\] -- 2026-02-17

-   Added Playlist-based playback system
-   Added SetPlaylists()
-   Added PlayPlaylist()
-   Added SetPremiumStatus()
-   Updated integration flow
-   Updated documentation

## \[1.1.0\] -- 2025-06-12

-   Fixed video freeze issue
-   Improved playback stability
-   Improved progress bar UI

## \[1.0.0\] -- 2025-06-09

-   Fixed layout issues in landscape
-   Controls auto-hide improvements
-   Fixed resume freeze issue
