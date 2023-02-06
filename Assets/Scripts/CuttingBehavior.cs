using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class CuttingBehavior : MonoBehaviour
{
    public int cuts = 5;
    private int _cutsDone = 0;

    public GameObject cutObject;
    [Tooltip("Attatch transform for the cutObject")]
    private Transform _attatchTransform;

    public string ingredient;

    public Action<Collider> OnTriggerEnterAction { get; set; }
    public Action<Collider> OnTriggerExitAction { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterAction?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitAction?.Invoke(other);
    }


    public void Cut(Transform position)
    {
        if (++_cutsDone == cuts)
        {
            Destroy(gameObject);
            return;
        }

        GameObject lettuce = Instantiate(cutObject);
        lettuce.transform.SetPositionAndRotation(position.position, position.rotation);
    }
}