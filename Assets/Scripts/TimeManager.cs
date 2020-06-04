using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for handling simulation time
/// </summary>
public class TimeManager : MonoBehaviour
{
    public static bool isPaused = true;
    public Button pauseButton;
    public Button playButton;
    public Text sliderText;

    private float speed; // Playback speed

    // Start is called before the first frame update
    private void Start() {
        Pause();
        pauseButton.interactable = false;
        playButton.interactable = true;
        speed = 1f;
    }

    /// <summary>
    /// Pauses simulation
    /// </summary>
    public void Pause() {
        Time.timeScale = 0f;
        isPaused = true;

        // Switch button states accordingly
        pauseButton.interactable = false;
        playButton.interactable = true;

        Debug.Log("Simulation paused");
    }

    /// <summary>
    /// Resumes simulation
    /// </summary>
    public void Resume() {
        Time.timeScale = speed;
        isPaused = false;

        // Switch button states accordingly
        pauseButton.interactable = true;
        playButton.interactable = false;

        Debug.Log("Simulation resumed");
    }

    /// <summary>
    /// Changes playback speed
    /// </summary>
    /// <param name="newSpeed"></param>
    public void ChangeSpeed(float newSpeed) {
        speed = newSpeed;
        Time.timeScale = speed;

        sliderText.text = speed.ToString("F2") + "X";
    }
}
