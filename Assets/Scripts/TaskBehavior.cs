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

    public bool TaskActive { get; private set; }

    public void Init()
    {
        foreach (GameObject obj in _gameObjects)
        {
            XRGrabInteractable interactable = obj.GetComponent<XRGrabInteractable>();
            if(interactable == null)
            {
                continue;
            }

            interactable.enabled = false;
            var marker = GetMarker(obj);
            if (marker != null)
            {
                marker.SetActive(false);
                _markers.Add(marker);
            }
        }

        foreach (GameObject marker in _markers)
        {
            marker.SetActive(false);
        }
    }

    public void AddObjectToTask(GameObject gameObject)
    {
        _gameObjects.Add(gameObject);
        var interactable = gameObject.GetComponent<XRGrabInteractable>();
        var marker = GetMarker(gameObject);
        if (marker is null)
        {
            Debug.LogWarning("Marker is null");
            return;
        }
        _markers.Add(marker);
        marker.SetActive(TaskActive);
    }

    public void RemoveObjectFromTask(GameObject gameObject)
    {
        _gameObjects.Remove(gameObject);
        var interactable = gameObject.GetComponent<XRGrabInteractable>();
        var marker = GetMarker(gameObject);
        if (marker is null)
        {
            Debug.LogWarning("Marker is null");
            return;
        }
        _markers.Remove(marker);
        marker.SetActive(false);
    }

    public void StartTask()
    {
        TaskActive = true;

        foreach (XRGrabInteractable interactable in _gameObjects.Select(x => x.GetComponent<XRGrabInteractable>()).Where(x => x != null))
        {
            interactable.enabled = true;
        }

        foreach (GameObject marker in _markers)
        {
            marker.SetActive(true);
        }
    }

    public void FinishTask()
    {
        TaskActive = false;

        foreach (GameObject marker in _markers)
        {
            try
            {
                marker.SetActive(false);
            }
            catch
            {
                // Fuck you
            }

        }
    }


    GameObject GetMarker(GameObject obj)
    {
        return obj.GetComponentsInChildren<Transform>(true)
            .Select(x => x.gameObject)
            .SingleOrDefault(x => x.name == "Marker");
    }
}