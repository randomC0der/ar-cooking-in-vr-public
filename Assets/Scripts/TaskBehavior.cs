using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEditor;

public class TaskBehavior : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _gameObjects = new List<GameObject>();

    [SerializeField]
    private List<GameObject> _markers;

    [SerializeField]
    public string TaskText;

    public Task Task;

    public void Init()
    {
        foreach (XRGrabInteractable interactable in _gameObjects.Select(x => x.GetComponent<XRGrabInteractable>())
            .Where(x => x != null))
        {
            interactable.enabled = false;
            var marker = interactable.gameObject.GetNamedChild("Marker");
            if(marker != null)
            {
                marker.SetActive(false);
                _markers.Add(marker);
            }
        }

        foreach(GameObject marker in _markers)
        {
            marker.SetActive(false);
        }
    }

    public void StartTask()
    {
        foreach (XRGrabInteractable interactable in _gameObjects.Select(x => x.GetComponent<XRGrabInteractable>()).Where(x => x != null))
        {
            interactable.enabled = true;
        }

        foreach (GameObject marker in _markers)
        {
            marker.SetActive(true);
        }
    }
}