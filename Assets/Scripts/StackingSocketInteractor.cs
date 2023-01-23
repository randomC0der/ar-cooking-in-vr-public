using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StackingSocketInteractor : XRSocketInteractor
{
    internal Transform tran;

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        var i2 = (XRBaseInteractable) interactable;

        if (i2 is null)
        {
            Debug.LogError($"{nameof(i2)} is null");
            return false;
        }

        bool h = i2.isSelected && i2.interactorsSelecting[0].transform.gameObject.name.Contains("Ray");
        return !hasSelection && h && base.CanHover(interactable);
    }

}
