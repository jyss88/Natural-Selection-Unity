using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static bool isPaused = false;
    public Button pauseButton;
    public Button playButton;
    public Button fastButton;

    private void Start() {
        Pause();
        pauseButton.interactable = false;
        playButton.interactable = true;
        fastButton.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void Pause() {
        Time.timeScale = 0f;
        isPaused = true;

        pauseButton.interactable = false;
        playButton.interactable = true;
        fastButton.interactable = true;

        Debug.Log("Simulation paused");
    }

    public void Resume() {
        Time.timeScale = 1f;
        isPaused = false;

        pauseButton.interactable = true;
        playButton.interactable = false;
        fastButton.interactable = true;

        Debug.Log("Simulation resumed");
    }

    public void DoubleSpeed() {
        Time.timeScale = 2f;
        isPaused = false;

        pauseButton.interactable = true;
        playButton.interactable = true;
        fastButton.interactable = false;

        Debug.Log("Double speed activated");
    }
}
