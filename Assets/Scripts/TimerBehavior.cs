using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimerBehavior : MonoBehaviour
{
    public Color color1;
    public Color color2;

    public double TimeRemaining
    {
        get => _timer.timeRemaining;
        set => _timer.timeRemaining = value;
    }

    public void SetTimer(double value)
    {
        _timer.minutes = (int)value / 60;
        _timer.seconds = ((int)value) % 60;
    }

    public bool Visible
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }

    private Timer _timer;

    // Start is called before the first frame update
    void Start()
    {
        _timer = FindObjectOfType<Timer>();
    }

    [ContextMenu(nameof(StartRunning))]
    public void StartRunning()
    {
        _timer.StartTimer();
    }

    [ContextMenu(nameof(PauseRunning))]
    public void PauseRunning()
    {
        _timer.StopTimer();
    }
}