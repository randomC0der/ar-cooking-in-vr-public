using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRDirectInteractor))]
public class HoverHighlighter : MonoBehaviour
{
    XRDirectInteractor xrInteractor;

    [SerializeField]
    Shader highlightShader;

    IXRInteractable lastHighlighted;
    Color lastHighlightedColor;

    Color color = Color.white;

    object _highlightSync = new object();
    object _unhighlightSync = new object();

    // Start is called before the first frame update
    void Start()
    {
        xrInteractor = GetComponent<XRDirectInteractor>();

        xrInteractor.hoverEntered.AddListener(HoverEntered);
        xrInteractor.hoverExited.AddListener(HoverExited);

        xrInteractor.selectEntered.AddListener(SelectEntered);
        xrInteractor.selectExited.AddListener(SelectExited);
    }

    private void HoverEntered(HoverEnterEventArgs e)
    {
        Highlight((IXRInteractable)xrInteractor.firstInteractableSelected ?? e.interactableObject);
    }
    private void SelectEntered(SelectEnterEventArgs e)
    {
        Unhightlight(e.interactableObject);
    }
    private void SelectExited(SelectExitEventArgs e)
    {
        Highlight((IXRInteractable)xrInteractor.firstInteractableSelected ?? e.interactableObject);
    }
    private void HoverExited(HoverExitEventArgs e)
    {
        Unhightlight(e.interactableObject);
    }


    private void Highlight(IXRInteractable interactable)
    {
        if (interactable == null)
        {
            return;
        }

        lock (interactable)
        {
            var gobj = interactable.transform.gameObject;
            var rend = gobj.GetComponent<Renderer>() ?? gobj.GetComponentInChildren<Renderer>();
            if (lastHighlighted == interactable || rend == null)
            {
                return;
            }

            //Unhighlight last
            Unhightlight(lastHighlighted);

            //Highlight current
            rend.material.EnableKeyword("_EMISSION");
            rend.material.SetColor("_EmissionColor", color);

            //Save current as last
            lastHighlighted = interactable;
        }
    }

    private void Unhightlight(IXRInteractable interactable)
    {
        if (interactable == null)
        {
            return;
        }

        lock (interactable)
        {
            var rend = interactable.transform.gameObject.GetComponent<Renderer>();
            if (rend == null)
            {
                return;
            }

            rend.material.DisableKeyword("_EMISSION");
            lastHighlighted = null;
        }
    }
}
