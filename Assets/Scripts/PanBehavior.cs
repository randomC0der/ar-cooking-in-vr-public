using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PanBehavior : MonoBehaviour
{
    TimerBehavior _timer;

    // Start is called before the first frame update
    void Start()
    {
        _timer = FindObjectOfType<TimerBehavior>();
        _timer.Visible = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnterPanSnap(SelectEnterEventArgs e)
    {
        CookableBehavior cookable = e.interactableObject.transform.gameObject.GetComponent<CookableBehavior>();
        if (cookable is null)
        {
            Debug.LogError($"{nameof(cookable)} is null");
            return;
        }

        cookable.StartCooking();
        cookable.CookingAudioSource.Play();

        if (cookable.Done.HasValue)
        {
            _timer.StartRunning();
        }

        if (cookable.Done == true)
        {
            _timer.SetTimer(cookable.overCookingTime);
            _timer.TimeRemaining = (cookable.overCookingTime + cookable.cookingTime - cookable.PassedTime);
        }
        else if (cookable.Done == false)
        {
            _timer.SetTimer(cookable.cookingTime);
            _timer.TimeRemaining = (cookable.cookingTime - cookable.PassedTime);
        }

        _timer.Visible = true;
    }

    public void ExitPanSnap(SelectExitEventArgs e)
    {
        CookableBehavior cookable = e.interactableObject.transform.gameObject.GetComponent<CookableBehavior>();
        if (cookable is null)
        {
            Debug.LogError($"{nameof(cookable)} is null");
            return;
        }

        cookable.StopCooking();
        cookable.CookingAudioSource.Stop();
        _timer.Visible = false;
        _timer.PauseRunning();
    }
}