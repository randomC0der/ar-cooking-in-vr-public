using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapBehavior : MonoBehaviour
{
    [field: SerializeField]
    public AudioSource AudioSource { get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnEnterSelection(SelectEnterEventArgs e)
    {
        AudioSource.Play();
    }

    // please note that if the interactableObject is so positioned that after applying the attatch transform
    // it gets out of the collider, both events are triggered all the time resulting in glitching/teleportation

    public void OnEnterHover(HoverEnterEventArgs e)
    {
        Transform t = e.interactableObject.transform;
        var grab = (XRGrabInteractable)e.interactableObject;
        grab.attachTransform = t.Find("Attatch Transform");
    }

    public void OnExitHover(HoverExitEventArgs e)
    {
        var grab = (XRGrabInteractable)e.interactableObject;
        grab.attachTransform = null;
    }
}
