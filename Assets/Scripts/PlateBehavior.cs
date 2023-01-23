using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlateBehavior : MonoBehaviour
{
    private GameObject _snapSpace;
    private List<SnapSpace> _children = new List<SnapSpace>();

    // Start is called before the first frame update
    void Start()
    {
        _snapSpace = Resources.Load<GameObject>("Snap Space");

        //children = Enumerable.Range(0, transform.childCount)
        //    .Select(i => transform.GetChild(i).gameObject)
        //    .Select(x => new SnapSpace
        //    {
        //        GameObject = x,
        //        Interactor = x.GetComponent<StackingSocketInteractor>()
        //    })
        //    .OrderBy(x => x.GameObject.name)
        //    .ToList();

        //foreach (var child in children)
        //{
        //    child.Interactor.socketActive = false;

        

        
        //}
        //children[0].Interactor.socketActive = true;
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
