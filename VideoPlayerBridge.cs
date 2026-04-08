using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class VideoPlayerBridge : MonoBehaviour
{
    #region P/Invoke - Native Swift Functions

    [DllImport("__Internal")]
    private static extern void loadVideo(string url);

    [DllImport("__Internal")]
    private static extern void play();

    [DllImport("__Internal")]
    private static extern void pause();

    [DllImport("__Internal")]
    private static extern void stop();

    [DllImport("__Internal")]
    private static extern void seekForward(double seconds);

    [DllImport("__Internal")]
    private static extern void seekBackward(double seconds);

    [DllImport("__Internal")]
    private static extern void seekTo(double value);

    [DllImport("__Internal")]
    private static extern void cleanup();

    [DllImport("__Internal")]
    private static extern void setURLS(IntPtr urlArray, int count);

    [DllImport("__Internal")]
    private static extern void setShowForwardButton(bool visible);

    [DllImport("__Internal")]
    private static extern void setShowBackwordButton(bool visible);

    [DllImport("__Internal")]
    private static extern void setShowBack10Button(bool visible);

    [DllImport("__Internal")]
    private static extern void setShowFor10Button(bool visible);

    [DllImport("__Internal")]
    private static extern void setShowPlayPauseButton(bool visible);

    [DllImport("__Internal")]
    private static extern void setShowBackButton(bool visible);

    [DllImport("__Internal")]
    private static extern void setShowLogo(bool visible);

    [DllImport("__Internal")]
    private static extern void setShowSeekbar(bool visible);

    [DllImport("__Internal")]
    private static extern void setShowTimeDuration(bool visible);

    [DllImport("__Internal")]
    private static extern void SetPlaylists(string json);

    [DllImport("__Internal")]
    private static extern void SetEpisodes(string json);

    [DllImport("__Internal")]
    private static extern void PlayPlaylist(string playlistId);

    [DllImport("__Internal")]
    private static extern void SetPremium(bool value);

    [DllImport("__Internal")]
    private static extern void SetAdRequired(bool value);

    [DllImport("__Internal")]
    private static extern void AdCompletedResumePlayback();

    [DllImport("__Internal")]
    private static extern void SetMidRollAdConfig(string json);

    [DllImport("__Internal")]
    private static extern void registerUnityCallback(UnityCallback callback);

    #endregion

    #region Delegate Callback from Swift

    public delegate void UnityCallback(string message);

    /// <summary>
    /// C# events that game code can subscribe to for video player events.
    /// Usage: VideoPlayerBridge.OnAdRequired += () => { ShowYourAd(); };
    /// </summary>
    public static event Action OnAdRequired;
    public static event Action OnUserExit;
    public static event Action<string> OnVideoFinished;
    public static event Action<string> OnVideoTileClicked;
    public static event Action<string> OnNextVideoAdRequired;
    public static event Action OnCallbackRegistered;
    public static event Action<string> OnVideoPlay;
    public static event Action<string> OnVideoPause;
    public static event Action<string> OnVideoStop;
    public static event Action<string> OnMidRollAdRequired;

    /// <summary>
    /// Called by Swift SDK via the delegate function pointer.
    /// Message format: "EventName:value"
    /// </summary>
    [AOT.MonoPInvokeCallback(typeof(UnityCallback))]
    public static void OnSwiftEvent(string message)
    {
        Debug.Log("[VideoPlayerBridge] Swift Event: " + message);

        if (string.IsNullOrEmpty(message)) return;

        // Parse "EventName:value"
        string eventName = message;
        string value = "";
        int colonIndex = message.IndexOf(':');
        if (colonIndex >= 0)
        {
            eventName = message.Substring(0, colonIndex);
            value = message.Substring(colonIndex + 1);
        }

        switch (eventName)
        {
            case "OnViewDismissed":
                if (value == "ad_required")
                {
                    Debug.Log("[VideoPlayerBridge] >> AD REQUIRED — show ad now!");
                    OnAdRequired?.Invoke();
                }
                else if (value == "user_exit")
                {
                    Debug.Log("[VideoPlayerBridge] >> User exited video player");
                    OnUserExit?.Invoke();
                }
                break;

            case "OnVideoFinished":
                Debug.Log("[VideoPlayerBridge] >> Video finished at index: " + value);
                OnVideoFinished?.Invoke(value);
                break;

            case "OnNextVideoAdRequired":
                Debug.Log("[VideoPlayerBridge] >> Ad required before next video at index: " + value);
                OnNextVideoAdRequired?.Invoke(value);
                break;

            case "OnMidRollAdRequired":
                Debug.Log("[VideoPlayerBridge] >> Mid-roll ad required at time: " + value);
                OnMidRollAdRequired?.Invoke(value);
                OnAdRequired?.Invoke();
                break;

            case "OnVideoTileClicked":
                Debug.Log("[VideoPlayerBridge] >> Video tile clicked: " + value);
                OnVideoTileClicked?.Invoke(value);
                break;

            case "CallBack_registered":
                Debug.Log("[VideoPlayerBridge] >> Callback registered with Swift SDK");
                OnCallbackRegistered?.Invoke();
                break;

            case "OnVideoPlay":
                OnVideoPlay?.Invoke(value);
                break;

            case "OnVideoPause":
                OnVideoPause?.Invoke(value);
                break;

            case "OnVideoStop":
                OnVideoStop?.Invoke(value);
                break;

            case "OnVideoClosed":
                Debug.Log("[VideoPlayerBridge] >> Video closed: " + value);
                OnUserExit?.Invoke();
                break;

            default:
                Debug.Log("[VideoPlayerBridge] >> Unhandled event: " + eventName + " value: " + value);
                break;
        }
    }

    #endregion

    #region Initialization

    void Start()
    {
#if UNITY_IOS && !UNITY_EDITOR
        registerUnityCallback(OnSwiftEvent);
        Debug.Log("[VideoPlayerBridge] Callback registered with Swift SDK");
#endif
    }

    #endregion

    #region Public API — Call These From Your Game Code

    public void LoadVideo(string url) { loadVideo(url); }
    public void Play() { play(); }
    public void Pause() { pause(); }
    public void Stop() { stop(); }
    public void SeekForward(double seconds) { seekForward(seconds); }
    public void SeekBackward(double seconds) { seekBackward(seconds); }
    public void SeekTo(double value) { seekTo(value); }
    public void Cleanup() { cleanup(); }

    public void SetURLs(string[] urls)
    {
        IntPtr urlArray = MarshalArray(urls);
        setURLS(urlArray, urls.Length);
        Marshal.FreeHGlobal(urlArray);
    }

    public void ShowForwardButton(bool visible) { setShowForwardButton(visible); }
    public void ShowBackwordButton(bool visible) { setShowBackwordButton(visible); }
    public void ShowBack10Button(bool visible) { setShowBack10Button(visible); }
    public void ShowFor10Button(bool visible) { setShowFor10Button(visible); }
    public void ShowPlayPauseButton(bool visible) { setShowPlayPauseButton(visible); }
    public void ShowBackButton(bool visible) { setShowBackButton(visible); }
    public void ShowLogo(bool visible) { setShowLogo(visible); }
    public void ShowSeekbar(bool visible) { setShowSeekbar(visible); }
    public void ShowTimeDuration(bool visible) { setShowTimeDuration(visible); }

    /// <summary>
    /// Set episodes data. Call BEFORE SetPlaylistData().
    /// </summary>
    public void SetEpisodesData(string json) { SetEpisodes(json); }

    /// <summary>
    /// Set playlist metadata. Call AFTER SetEpisodesData().
    /// </summary>
    public void SetPlaylistData(string json) { SetPlaylists(json); }

    /// <summary>
    /// Start playing a playlist by ID. Call after SetEpisodesData() and SetPlaylistData().
    /// </summary>
    public void PlayPlaylistById(string playlistId) { PlayPlaylist(playlistId); }

    /// <summary>
    /// Set premium status. true = no ads, false = ads enabled.
    /// Call BEFORE PlayPlaylistById().
    /// </summary>
    public void SetPremiumStatus(bool isPremium) { SetPremium(isPremium); }

    /// <summary>
    /// Set whether an ad is required before playback.
    /// </summary>
    public void SetAdRequiredStatus(bool isRequired) { SetAdRequired(isRequired); }

    /// <summary>
    /// Call this AFTER your ad finishes to resume video playback in the SDK.
    /// Works for pre-roll, inter-video, and mid-roll ads.
    /// </summary>
    public void ResumeAfterAd() { AdCompletedResumePlayback(); }

    /// <summary>
    /// Set the mid-roll ad configuration JSON. Must be called BEFORE PlayPlaylistById().
    /// The config controls duration buckets, placement percentages, per-video overrides,
    /// and global constraints (min gap, no-ad zones, timeout).
    /// Pass the raw JSON string from your backend.
    /// </summary>
    public void SetMidRollAdConfigJson(string json) { SetMidRollAdConfig(json); }

    #endregion

    #region Helper

    private IntPtr MarshalArray(string[] array)
    {
        IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size * array.Length);
        for (int i = 0; i < array.Length; i++)
        {
            IntPtr stringPtr = Marshal.StringToHGlobalAnsi(array[i]);
            Marshal.WriteIntPtr(ptr, i * IntPtr.Size, stringPtr);
        }
        return ptr;
    }

    #endregion
}