using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StackingSocketInteractor : XRSocketInteractor
{
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        var baseInteractable = (XRBaseInteractable)interactable;

        if (baseInteractable is null)
        {
            Debug.LogError($"{nameof(baseInteractable)} is null");
            return false;
        }

        bool hover = !baseInteractable.isSelected || (baseInteractable.isSelected && baseInteractable.interactorsSelecting[0].GetType() != GetType());
        return !hasSelection && hover && base.CanHover(interactable);
    }

}
