using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CuttingBehavior : MonoBehaviour
{
    public int cuts = 5;
    private int _cutsDone = 0;

    public GameObject cutObject;
    [Tooltip("Attatch transform for the cutObject")]
    private Transform _attatchTransform;

    GrabableGameObject grabable;

    // Start is called before the first frame update
    void Start()
    {
        grabable = gameObject.AddGrabableComponents();
        grabable.XrGrab.interactionLayers = InteractionLayerMask.GetMask("Default", "Cuttable");
        _attatchTransform = transform.Find("AttatchTransformCut");
    }


    public void Cut()
    {
        if (++_cutsDone == cuts)
        {
            Destroy(gameObject);
            return;
        }
        GameObject lettuce = Instantiate(cutObject);

        if (_attatchTransform != null)
        {
            _attatchTransform.AttatchTo(lettuce.transform);
        }

        GrabableGameObject grabableLettuce = lettuce.AddGrabableComponents();
        grabableLettuce.Rigidbody.mass = .1f;
        lettuce.transform.position = gameObject.transform.position;
        lettuce.transform.localScale = gameObject.transform.localScale;
    }
}