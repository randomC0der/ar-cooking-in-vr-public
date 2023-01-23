using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnterSelection(HoverEnterEventArgs e)
    {
        Transform t = e.interactableObject.transform;
        var grab = t.gameObject.GetComponent<XRGrabInteractable>();
        grab.attachTransform = t.Find("Attatch Transform");
        //grab.matchAttachPosition = false;
        //Debug.Log(t.gameObject.GetComponent<XRGrabInteractable>().attachTransform);
    }

    public void OnExitSelection(HoverExitEventArgs e)
    {
        Transform t = e.interactableObject.transform;
        var grab = t.gameObject.GetComponent<XRGrabInteractable>();
        //var help = new GameObject();
        grab.attachTransform = null;
        //grab.matchAttachPosition = true;
        //Debug.Log(t.gameObject.GetComponent<XRGrabInteractable>().attachTransform);
    }
}
