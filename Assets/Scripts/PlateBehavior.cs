using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlateBehavior : MonoBehaviour
{
    private GameObject _snapSpace;
    private List<SnapSpace> _children = new List<SnapSpace>();
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _snapSpace = Resources.Load<GameObject>("Snap Space");
        _audioSource = gameObject.GetComponent<AudioSource>();

        CreateSnapSpace();
    }

    SnapSpace CreateSnapSpace()
    {
        var snapSpace = Instantiate(_snapSpace, transform);
        snapSpace.name = $"SnapSpace {_children.Count}";

        var child = new SnapSpace
        {
            GameObject = snapSpace,
            Interactor = snapSpace.GetComponent<StackingSocketInteractor>()
        };

        var selectEnterEvent = new SelectEnterEvent();
        selectEnterEvent.AddListener(EnableNextLevel);
        child.Interactor.selectEntered = selectEnterEvent;

        var selectExitEvent = new SelectExitEvent();
        selectExitEvent.AddListener(DisableNextLevel);
        child.Interactor.selectExited = selectExitEvent;

        _children.Add(child);

        return child;
    }

    void EnableNextLevel(SelectEnterEventArgs e)
    {
        _audioSource.Play();
        if (_children.All(x => x.Interactor.hasSelection))
        {
            var snapSpace = CreateSnapSpace();
            snapSpace.GameObject.transform.Translate(new Vector3(0, .01f * _children.Count));
            return;
        }

        var nextItem = _children.Select((x, i) => (x, i)).FirstOrDefault(x => !x.x.Interactor.socketActive);
        if (nextItem.x.GameObject == null) // default
        {
            Debug.LogWarning("gameobject is null");
            return;
        }
        _children[nextItem.i].Interactor.socketActive = true;
    }

    void DisableNextLevel(SelectExitEventArgs e)
    {
        if (_children.Any(x => x.Interactor.socketActive && !x.Interactor.hasSelection && x.Interactor.transform != e.interactorObject.transform))
        {
            ((XRSocketInteractor)e.interactorObject).socketActive = false;
        }
    }

    struct SnapSpace
    {
        public GameObject GameObject;
        public StackingSocketInteractor Interactor;
    }
}
