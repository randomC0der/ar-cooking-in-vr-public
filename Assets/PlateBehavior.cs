using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlateBehavior : MonoBehaviour
{
    List<SnapSpace> children;

    // Start is called before the first frame update
    void Start()
    {
        children = Enumerable.Range(0, transform.childCount)
            .Select(i => transform.GetChild(i).gameObject)
            .Select(x => new SnapSpace
            {
                GameObject = x,
                Interactor = x.GetComponent<StackingSocketInteractor>()
            })
            .OrderBy(x => x.GameObject.name)
            .ToList();

        foreach (var child in children)
        {
            child.Interactor.socketActive = false;

            var selectEnterEvent = new SelectEnterEvent();
            selectEnterEvent.AddListener(EnableNextLevel);
            child.Interactor.selectEntered = selectEnterEvent;

            var selectExitEvent = new SelectExitEvent();
            selectExitEvent.AddListener(DisableNextLevel);
            child.Interactor.selectExited = selectExitEvent;
        }
        children[0].Interactor.socketActive = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void EnableNextLevel(SelectEnterEventArgs e)
    {
        var nextItem = children.Select((x, i) => (x, i)).FirstOrDefault(x => !x.x.Interactor.socketActive);
        if (nextItem.x.GameObject == null) // default
        {
            Debug.LogWarning("gameobject is null");
            return;
        }
        if (nextItem.i + 1 != children.Count)
        {
            var go = e.interactableObject.transform.gameObject;
            var grab = go.GetComponent<XRGrabInteractable>();
            var body = go.GetComponent<Rigidbody>();
            children[nextItem.i].Interactor.socketActive = true;
        }
    }

    void DisableNextLevel(SelectExitEventArgs e)
    {
        if (children.Any(x => x.Interactor.socketActive && !x.Interactor.hasSelection))
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
