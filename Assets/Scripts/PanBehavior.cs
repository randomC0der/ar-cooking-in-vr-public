using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PanBehavior : MonoBehaviour
{
    [SerializeField] private TimerBehavior _timer;

    // Start is called before the first frame update
    void Start()
    {
        _timer.Visible = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnterPanSnap(SelectEnterEventArgs e)
    {
        var cookable = e.interactableObject.transform.gameObject.GetComponent<CookingParentBehavior>();
        if (cookable is null)
        {
            Debug.LogError($"{nameof(cookable)} is null");
            return;
        }

        cookable.StartCooking();
        cookable.OnCookingStatusChanged = UpdateTimer;
        cookable.XrGrab.attachTransform = cookable.transform.Find("Attatch Transform");

        UpdateTimer(cookable);

        _timer.Visible = true;
    }

    private void UpdateTimer(CookingParentBehavior cookable)
    {
        _timer.StartRunning();

        if (cookable.Done == true)
        {
            _timer.SetTimer(cookable.overCookingTime);
            _timer.TimeRemaining = cookable.overCookingTime + cookable.cookingTime - cookable.PassedTime;
        }
        else if (cookable.Done == false)
        {
            _timer.SetTimer(cookable.cookingTime);
            _timer.TimeRemaining = cookable.cookingTime - cookable.PassedTime;
        }
        else
        {
            _timer.SetTimer(1);
            _timer.TimeRemaining = 0.01;
        }
    }

    public void ExitPanSnap(SelectExitEventArgs e)
    {
        CookingParentBehavior cookable = e.interactableObject.transform.gameObject.GetComponent<CookingParentBehavior>();
        if (cookable is null)
        {
            Debug.LogError($"{nameof(cookable)} is null");
            return;
        }

        cookable.XrGrab.attachTransform = null;
        cookable.StopCooking();
        _timer.Visible = false;
        _timer.PauseRunning();
    }
}