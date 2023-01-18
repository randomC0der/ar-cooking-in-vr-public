using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingBehavior : MonoBehaviour
{
    public GameObject cookedItem;
    public GameObject fire;
    public float cookingTime = 10;
    public float overCookingTime = 10;


    private GameObject _parent;
    private CookingParentBehavior _parentBehavior;

    // Start is called before the first frame update
    void Start()
    {
        _parent = new GameObject();
        _parentBehavior = _parent.AddComponent<CookingParentBehavior>()
            .PassParameter(gameObject, cookedItem, fire, cookingTime, overCookingTime);
        transform.parent = _parent.transform;
        gameObject.AddGrabableComponents();
    }

    [ContextMenu("StartCooking")]
    public void StartCooking()
    {
        _parentBehavior.IsCooking = true;
    }

    [ContextMenu("StopCooking")]
    public void StopCooking()
    {
        _parentBehavior.IsCooking = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        var go = collision.gameObject;

        if (go.name != "pan")
        {
            return;
        }

        StartCooking();
    }

    void OnCollisionExit(Collision collision)
    {
        var go = collision.gameObject;

        if (go.name != "pan")
        {
            return;
        }

        StopCooking();
    }

    // Update is called once per frame
    void Update()
    {
        _parent.transform.position = transform.position;
        transform.localPosition = Vector3.zero;
    }
}