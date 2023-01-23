using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PanBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

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
    }
}