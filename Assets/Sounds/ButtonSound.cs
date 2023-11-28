using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    //[SerializeField] AudioClip soundClip;
    private AudioSource audioSource;
    //private Button button;

    //private void Start()
    //{
    //    // Get the AudioSource component attached to the GameObject
    //    audioSource = GetComponent<AudioSource>();
    //    // Set the audio clip to play
    //    audioSource.clip = soundClip;

    //    // Get the Button component
    //    button = GetComponentInChildren<Button>();

    //    // Add a listener to the button's click event
    //    button.onClick.AddListener(PlaySound);
    //}

    public void PlaySound()
    {
        // Play the audio clip when the button is clicked
        audioSource.Play();
    }
}