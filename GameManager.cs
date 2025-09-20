using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public void OnVideoPlay(string msg)
    {
        Debug.Log("Unity received play event: " + msg);
        // Add unity logic here
        
    }

  
    public void OnVideoPause(string msg)
    {
        Debug.Log("Unity received pause event: " + msg);
        // Add unity logic here
    }

    
    public void OnVideoStop(string msg)
    {
        Debug.Log("Unity received stop event: " + msg);
        // Add unity logic here
    }

   
    public void OnVideoSeekForward(string msg)
    {
        Debug.Log("Unity received seek forward event: " + msg + " seconds");
        // Add unity logic here
    }

    public void OnVideoSeekBackward(string msg)
    {
        Debug.Log("Unity received seek backward event: " + msg + " seconds");
        // Add unity logic here
    }
    
    public void OnVideoSeekTo(string msg)
    {
        Debug.Log("Unity received seek to event: " + msg + " seconds");
        // Add unity logic here
    }

    public void OnVideoClosed(string msg)
    {
        Debug.Log("Unity received back press / closed event: " + msg);
        // Add unity logic here
    }

    public void OnVideoFinished(string msg)
    {
        Debug.Log("Unity received video finished event: " + msg);
        // Add unity logic here
    }
}
