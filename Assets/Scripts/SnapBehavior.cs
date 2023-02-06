using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapBehavior : MonoBehaviour
{
    [field: SerializeField]
    public AudioSource AudioSource { get; set; }

    public void OnEnterSelection(SelectEnterEventArgs e)
    {
        AudioSource.Play();
    }

    // please note that if the interactableObject is so positioned that after applying the attatch transform
    // it gets out of the collider, both events are triggered all the time resulting in glitching/teleportation

    public void OnEnterHover(HoverEnterEventArgs e)
    {

    }

    public void OnExitHover(HoverExitEventArgs e)
    {
        
    }
}
