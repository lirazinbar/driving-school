using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class DelayedVideoStart : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public float delayInSeconds = 22f;

    private void Start()
    {
        // Start the coroutine to delay video playback
        StartCoroutine(StartVideoAfterDelay());
    }

    private IEnumerator StartVideoAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayInSeconds);

        // Start playing the video
        videoPlayer.Play();
    }
}