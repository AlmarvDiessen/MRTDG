using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button playButton;

    void Start()
    {
        // Assign the VideoPlayer component (you can do this through the Inspector as well)
        videoPlayer = GetComponent<VideoPlayer>();

        // Get the Button component
        playButton = GetComponentInChildren<Button>();

        // Add a listener to the button's click event
        playButton.onClick.AddListener(PlayVideo);
    }

    public void PlayVideo()
    {
        // Play the video when the button is clicked
        videoPlayer.Play();
    }
}