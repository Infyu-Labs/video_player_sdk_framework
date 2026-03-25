# 🎮 Video Player SDK for Unity (iOS)

This SDK bridges native Swift-based video playback functionality with
Unity using P/Invoke, enabling seamless playlist-based video playback
inside Unity applications.

------------------------------------------------------------------------

## 📦 Installation

1.  Clone the repository or drag the `.framework` into your Xcode Unity
    project.
2.  Ensure the framework is **Embedded & Signed** in:
    `Target → Frameworks, Libraries & Embedded Content`
3.  Place the framework inside: `Assets/Plugins/iOS`
4.  Enable Objective-C support in Unity iOS Build Settings (if
    required).
5.  Set the minimum deployment target to **iOS 15.0**.

------------------------------------------------------------------------

## 🎯 Key Features

-   Supports video formats: **HLS, MP4, MOV**
-   Playlist-based playback
-   Premium / Non-premium support
-   Unity-controlled Ad system
-   Playlist autoplay
-   Swift → Unity callbacks
-   Custom playback controls

Playback Controls:

-   Play / Pause / Stop
-   Seek Forward / Backward
-   Skip +10 / -10 seconds
-   Scrub slider
-   Next / Previous video
-   Exit / Back button
-   Auto-hide controls

------------------------------------------------------------------------

## 📺 Ad Integration (Unity Controlled)

The SDK does **NOT include any Ad SDK**.

Instead, it notifies Unity **when an Ad must be shown**. Unity is
responsible for showing the Ad and notifying the SDK when the Ad
completes.

This allows the Unity app to use **any Ad provider (AdMob, IronSource,
Unity Ads, etc.)**.

------------------------------------------------------------------------

## 💎 Premium vs Free Users

### Premium Users

-   No Ads
-   Videos play immediately
-   Playlist autoplay works normally

### Free Users

Ads must be shown in the following situations:

1.  Before starting the player
2.  When user taps another video
3.  When next video auto-plays

------------------------------------------------------------------------

## 🔄 Ad Flow

SDK requests Ad\
↓\
Unity receives callback\
↓\
Unity shows Ad\
↓\
Ad completes\
↓\
Unity calls `AdCompletedResumePlayback()`\
↓\
SDK resumes playback

------------------------------------------------------------------------

## 🎮 Unity Usage Example

``` csharp
public void PlayVideo()
{
    if (!isPremiumUser)
    {
        playerBridge.Play(); // SDK will wait for Ad

        ShowAd(() =>
        {
            playerBridge.AdCompletedResumePlayback();
        });
    }
    else
    {
        playerBridge.Play();
    }
}
```

------------------------------------------------------------------------

## 📡 Required Unity Callback Methods

### When user clicks another video

``` csharp
public void OnVideoTileClicked(string videoId)
{
    ShowAd(() =>
    {
        playerBridge.AdCompletedResumePlayback();
    });
}
```

### When next video auto-plays

``` csharp
public void OnNextVideoAdRequired(string index)
{
    ShowAd(() =>
    {
        playerBridge.AdCompletedResumePlayback();
    });
}
```

### When video finishes

``` csharp
public void OnVideoFinished(string index)
{
    Debug.Log("Video finished: " + index);
}
```

------------------------------------------------------------------------

## 🚀 Playlist Integration

Required order:

1.  `SetEpisodes(json)`
2.  `SetPlaylists(json)`
3.  `PlayPlaylist(playlistId)`

⚠️ `SetPlaylists()` and `SetEpisodes()`  must always be called before `PlayPlaylist()`.

------------------------------------------------------------------------

## 📘 Input JSON Format (Unity → iOS)

### 🎬 Episodes JSON

``` json
[
    {
    "id": "dinoworld",
    "episodes":[
            { 
            "index": 1,
            "url": "https://video1.m3u8", "thumbnail": "https://image.com/thumb1.png",
            "title": "Episode 1"
            }
        ]
    }
]
```

### 📂 Playlist Metadata JSON

``` json
[
    {
     "name": "DINO WORLD",
     "id": "dinoworld",
     "thumbnail":"https://image.com/thumb.png",
     "order": 1
    }
]

```

------------------------------------------------------------------------

## 📘 Playlist JSON Format

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

## 💎 Premium Control

``` csharp
playerBridge.SetPremium(true);   // Premium user
playerBridge.SetPremium(false);  // Free user
```

------------------------------------------------------------------------

## 🔁 Swift → Unity Callbacks

Messages are sent to the **SkidosVideoPlayer** GameObject.

  Action                   Unity Method            Message
  ------------------------ ----------------------- ---------------
  Play video               OnVideoPlay             "Started"
  Pause video              OnVideoPause            "Paused"
  Stop video               OnVideoStop             "Stopped"
  Seek forward             OnVideoSeekForward      "10.0"
  Seek backward            OnVideoSeekBackward     "10.0"
  Seek to time             OnVideoSeekTo           "120.0"
  Video finished           OnVideoFinished         "index"
  Video tile clicked       OnVideoTileClicked      "videoId"
  Next video ad required   OnNextVideoAdRequired   "index"
  Player closed            OnVideoClosed           "User exited"

------------------------------------------------------------------------

## 🧹 Cleanup

Always call cleanup when exiting the app.

``` csharp
void OnApplicationQuit()
{
    playerBridge.Cleanup();
}
```

------------------------------------------------------------------------

## 📝 Version History

### \[1.2.0\] --- 2026-03-07

Major update to Ad Integration.

Added:

-   Video tile click Ad callback
-   Auto next video Ad callback
-   Improved Unity controlled Ad workflow
-   Updated SDK documentation

New Callbacks:

-   `OnVideoTileClicked`
-   `OnNextVideoAdRequired`

------------------------------------------------------------------------

### \[1.1.1\] --- 2026-02-17

-   Added Playlist-based playback system
-   Added `SetPlaylists()`
-   Added `PlayPlaylist()`
-   Added `SetPremiumStatus()`
-   Added Unity-controlled Ad Flow

------------------------------------------------------------------------

### \[1.1.0\] --- 2025-06-12

-   Fixed video freeze issue
-   Improved playback stability
-   Improved progress bar UI

------------------------------------------------------------------------

### \[1.0.0\] --- 2025-06-09

-   Initial release
-   Landscape player support
-   Auto-hide controls
-   Resume playback fixes
