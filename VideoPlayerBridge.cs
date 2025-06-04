using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class VideoPlayerBridge : MonoBehaviour
{
    #region P/Invoke - Call Swift Functions
    
    // Load Video
    [DllImport("__Internal")]
    private static extern void loadVideo(string url);

    // Play Video
    [DllImport("__Internal")]
    private static extern void play();

    // Pause Video
    [DllImport("__Internal")]
    private static extern void pause();

    // Stop Video
    [DllImport("__Internal")]
    private static extern void stop();

    // Seek Forward
    [DllImport("__Internal")]
    private static extern void seekForward(double seconds);

    // Seek Backward
    [DllImport("__Internal")]
    private static extern void seekBackward(double seconds);

    // Seek to specific value
    [DllImport("__Internal")]
    private static extern void seekTo(double value);

    // Cleanup
    [DllImport("__Internal")]
    private static extern void cleanup();

    // Set Video URLs
    [DllImport("__Internal")]
    private static extern void setURLS(IntPtr urlArray, int count);

    // Set Visibility for Forward Button
    [DllImport("__Internal")]
    private static extern void setShowForwardButton(bool visible);

    // Set Visibility for Backward Button
    [DllImport("__Internal")]
    private static extern void setShowBackwordButton(bool visible);

    // Set Visibility for Back 10 Seconds Button
    [DllImport("__Internal")]
    private static extern void setShowBack10Button(bool visible);

    // Set Visibility for Forward 10 Seconds Button
    [DllImport("__Internal")]
    private static extern void setShowFor10Button(bool visible);

    // Set Visibility for Play/Pause Button
    [DllImport("__Internal")]
    private static extern void setShowPlayPauseButton(bool visible);

    // Set Visibility for Back Button
    [DllImport("__Internal")]
    private static extern void setShowBackButton(bool visible);

    // Set Visibility for Logo
    [DllImport("__Internal")]
    private static extern void setShowLogo(bool visible);

    // Set Visibility for Seekbar
    [DllImport("__Internal")]
    private static extern void setShowSeekbar(bool visible);

    // Set Visibility for Time Duration
    [DllImport("__Internal")]
    private static extern void setShowTimeDuration(bool visible);

    #endregion

    #region Wrapper Functions (C# Methods to Call from Unity)

    // Wrapper to load video
    public void LoadVideo(string url)
    {
        loadVideo(url);
    }

    // Wrapper to play video
    public void Play()
    {
        play();
    }

    // Wrapper to pause video
    public void Pause()
    {
        pause();
    }

    // Wrapper to stop video
    public void Stop()
    {
        stop();
    }

    // Wrapper to seek forward
    public void SeekForward(double seconds)
    {
        seekForward(seconds);
    }

    // Wrapper to seek backward
    public void SeekBackward(double seconds)
    {
        seekBackward(seconds);
    }

    // Wrapper to seek to specific time
    public void SeekTo(double value)
    {
        seekTo(value);
    }

    // Wrapper to cleanup video
    public void Cleanup()
    {
        cleanup();
    }

    // Wrapper to set URLs
    public void SetURLs(string[] urls)
    {
        IntPtr urlArray = MarshalArray(urls);
        setURLS(urlArray, urls.Length);
        Marshal.FreeHGlobal(urlArray);
    }

    // Wrapper to show/hide forward button
    public void ShowForwardButton(bool visible)
    {
        setShowForwardButton(visible);
    }

    // Wrapper to show/hide backward button
    public void ShowBackwordButton(bool visible)
    {
        setShowBackwordButton(visible);
    }

    // Wrapper to show/hide back 10 button
    public void ShowBack10Button(bool visible)
    {
        setShowBack10Button(visible);
    }

    // Wrapper to show/hide forward 10 button
    public void ShowFor10Button(bool visible)
    {
        setShowFor10Button(visible);
    }

    // Wrapper to show/hide play/pause button
    public void ShowPlayPauseButton(bool visible)
    {
        setShowPlayPauseButton(visible);
    }

    // Wrapper to show/hide back button
    public void ShowBackButton(bool visible)
    {
        setShowBackButton(visible);
    }

    // Wrapper to show/hide logo
    public void ShowLogo(bool visible)
    {
        setShowLogo(visible);
    }

    // Wrapper to show/hide seekbar
    public void ShowSeekbar(bool visible)
    {
        setShowSeekbar(visible);
    }

    // Wrapper to show/hide time duration
    public void ShowTimeDuration(bool visible)
    {
        setShowTimeDuration(visible);
    }

    #endregion

    #region Helper Method - Convert C# String Array to UnsafePointer

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
