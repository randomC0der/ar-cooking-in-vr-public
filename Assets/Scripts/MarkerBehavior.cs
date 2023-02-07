using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MarkerBehavior : MonoBehaviour
{
    private bool _previous;

    // Start is called before the first frame update
    void Start()
    {
        var grab = GetComponentInParent<XRGrabInteractable>();

        grab.selectEntered.AddListener(HideOnSelectionEnter);
        grab.selectExited.AddListener(ShowOnSelectionExit);

    }

    public void HideOnSelectionEnter(SelectEnterEventArgs e)
    {
        _previous = gameObject.activeSelf;
        gameObject.SetActive(false);
    }

    public void ShowOnSelectionExit(SelectExitEventArgs e)
    {
        gameObject.SetActive(_previous);
    }
}
