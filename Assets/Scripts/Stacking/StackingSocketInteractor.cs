using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StackingSocketInteractor : XRSocketInteractor
{
    internal Transform tran;

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        var baseInteractable = (XRBaseInteractable) interactable;

        if (baseInteractable is null)
        {
            Debug.LogError($"{nameof(baseInteractable)} is null");
            return false;
        }

        bool hover = baseInteractable.isSelected && baseInteractable.interactorsSelecting[0].transform.gameObject.name.Contains("Ray");
        return !hasSelection && hover && base.CanHover(interactable);
    }

}
