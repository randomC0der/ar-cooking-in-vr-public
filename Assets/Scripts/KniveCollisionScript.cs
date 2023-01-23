using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

public class KniveCollisionScript : MonoBehaviour
{
    [SerializeField]
    private KniveBehavior _kniveBehavior;
    private XRSocketInteractor _socketInteractor;

    private void Start()
    {
        _socketInteractor = GetComponent<XRSocketInteractor>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == _kniveBehavior.gameObject.name)
        {
            _kniveBehavior.EnterCuttingBoardCollision(_socketInteractor.firstInteractableSelected);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == _kniveBehavior.gameObject.name)
        {
            _kniveBehavior.ExitCuttingBoardCollision();
        }
    }
}
