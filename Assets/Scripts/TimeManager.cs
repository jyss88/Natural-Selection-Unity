using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static bool isPaused = true;
    public Button pauseButton;
    public Button playButton;
    public Text sliderText;

    private float speed;

    private void Start() {
        Pause();
        pauseButton.interactable = false;
        playButton.interactable = true;
        speed = 1f;
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

        Debug.Log("Simulation paused");
    }

    public void Resume() {
        Time.timeScale = speed;
        isPaused = false;

        pauseButton.interactable = true;
        playButton.interactable = false;

        Debug.Log("Simulation resumed");
    }

    public void ChangeSpeed(float newSpeed) {
        speed = newSpeed;
        Time.timeScale = speed;

        sliderText.text = speed.ToString("F2") + "X";
    }
}
