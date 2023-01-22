using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Inspired by https://www.youtube.com/watch?v=5xWDKJj1UGY
/// </summary>
public class TimerBehavior : MonoBehaviour
{
    public float indicatorTimer = 0;
    public float maxIndicatorTimer = 1;
    public float overMaxIndicatorTimer = 1;

    public Image radialIndicatorUI1;
    public Image radialIndicatorUI2;

    public bool running;

    // Start is called before the first frame update
    void Start()
    {
        radialIndicatorUI1.fillAmount = 0;
        radialIndicatorUI2.fillAmount = 0;
        StartRunning();
    }

    [ContextMenu("Start running")]
    public void StartRunning()
    {
        running = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!running)
        {
            return;
        }

        radialIndicatorUI1.fillAmount = indicatorTimer / maxIndicatorTimer;
        radialIndicatorUI2.fillAmount = (indicatorTimer - maxIndicatorTimer) / overMaxIndicatorTimer;

        if (indicatorTimer >= maxIndicatorTimer + overMaxIndicatorTimer)
        {
            running = false;
        }

        indicatorTimer += Time.deltaTime;
    }
}