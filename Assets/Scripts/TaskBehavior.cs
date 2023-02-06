using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using System.Linq;

public class TaskBehavior : MonoBehaviour
{
    [SerializeField]
    protected List<GameObject> _gameObjects = new List<GameObject>();

    [SerializeField]
    protected List<GameObject> _markers;

    public Task Task;

    public void Init()
    {
        foreach (XRGrabInteractable interactable in _gameObjects.Select(x => x.GetComponent<XRGrabInteractable>()))
        {
            interactable.enabled = false;
        }

        foreach(GameObject marker in _markers)
        {
            marker.SetActive(false);
        }
    }

    public void StartTask()
    {
        foreach (XRGrabInteractable interactable in _gameObjects.Select(x => x.GetComponent<XRGrabInteractable>()))
        {
            interactable.enabled = true;
        }

        foreach (GameObject marker in _markers)
        {
            marker.SetActive(true);
        }
    }
}