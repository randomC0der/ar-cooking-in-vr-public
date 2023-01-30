using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TrashcanBehavior : MonoBehaviour
{
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void EmptyTrashcan(SelectEnterEventArgs e)
    {
        _audioSource.PlayOneShot(_audioSource.clip);
        Transform t = e.interactableObject.transform;

        var trashable = t.GetComponent<TrashableBehavior>();
        if (trashable != null)
        {
            trashable.FinishTask();
        }

        var cookable = t.GetComponent<CookableBehavior>();
        Destroy(t.gameObject);
        if (cookable != null)
        {
            Destroy(cookable.Parent);
        }
    }
}