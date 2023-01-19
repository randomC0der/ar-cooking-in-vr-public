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
        Destroy(e.interactableObject.transform.gameObject);
    }
}
