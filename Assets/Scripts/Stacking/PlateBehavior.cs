using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.XR.Interaction.Toolkit;

// for the performance of the crafting see this video: https://www.youtube.com/watch?v=o4-zpAI7qBc
public class PlateBehavior : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    private GameObject _snapSpace;
    private readonly List<SnapSpace> _children = new List<SnapSpace>();
    private Receipe[] _receipes;

    // Start is called before the first frame update
    void Start()
    {
        _snapSpace = Resources.Load<GameObject>("Snap Space");

        TextAsset textAsset = Resources.Load<TextAsset>("Recipes");
        _receipes = JsonUtility.FromJson<ReceipeBook>(textAsset.text).recipes;

        CreateSnapSpace();
    }

    SnapSpace CreateSnapSpace()
    {
        var snapSpace = Instantiate(_snapSpace, transform).GetComponent<SnapBehavior>();
        snapSpace.name = $"SnapSpace {_children.Count}";
        snapSpace.AudioSource = _audioSource;

        var child = new SnapSpace
        {
            GameObject = snapSpace.gameObject,
            Interactor = snapSpace.GetComponent<StackingSocketInteractor>()
        };

        var selectEnterEvent = new SelectEnterEvent();
        selectEnterEvent.AddListener(EnableNextLevel);
        selectEnterEvent.AddListener(snapSpace.OnEnterSelection);
        child.Interactor.selectEntered = selectEnterEvent;

        var selectExitEvent = new SelectExitEvent();
        selectExitEvent.AddListener(DisableNextLevel);
        child.Interactor.selectExited = selectExitEvent;

        _children.Add(child);

        return child;
    }

    void EnableNextLevel(SelectEnterEventArgs e)
    {
        Transform t = e.interactableObject.transform;
        var grab = (XRGrabInteractable)e.interactableObject;
        grab.attachTransform = t.Find("Attatch Transform");

        // check for matching crafting recipe
        foreach (Receipe receipe in _receipes.Where(x => x.ingredients.Length == _children.Count(y => y.Interactor.hasSelection))
            .Where(receipe => IsReceipeFinished(receipe)))
        {
            TransformIngredients(receipe);
            return;
        }

        // add more socket interactors 
        if (_children.All(x => x.Interactor.hasSelection))
        {
            var snapSpace = CreateSnapSpace();
            snapSpace.GameObject.transform.Translate(new Vector3(0, .01f * _children.Count));
            return;
        }

        // enable next sockets
        var nextItem = _children.Select((x, i) => (x, i)).FirstOrDefault(x => !x.x.Interactor.socketActive);
        if (nextItem.x.GameObject == null) // default
        {
            Debug.LogWarning("gameobject is null");
            return;
        }
        _children[nextItem.i].Interactor.socketActive = true;
    }

    bool IsReceipeFinished(Receipe receipe)
    {
        for (int i = 0; i < receipe.ingredients.Length; i++)
        {
            if (_children[i].Interactor.firstInteractableSelected.transform.gameObject
                .GetComponent<StackableBehavior>().ingredient != receipe.ingredients[i])
            {
                return false;
            }
        }
        return true;
    }

    private void TransformIngredients(Receipe receipe)
    {
        GameObject product = Resources.Load<GameObject>(receipe.product);

        foreach (var gameobj in _children.Select(child => child.Interactor.firstInteractableSelected?.transform.gameObject)
            .Where(x => x != null))
        {
            Destroy(gameobj);
        }

        product = Instantiate(product);
        product.transform.position = transform.position;
    }

    void DisableNextLevel(SelectExitEventArgs e)
    {
        foreach (var i in _children.Select(x => x.Interactor).Where(x => !x.hasSelection))
        {
            i.socketActive = false;
        }
        _children.First(x => !x.Interactor.socketActive).Interactor.socketActive = true;
    }

    struct SnapSpace
    {
        public GameObject GameObject;
        public StackingSocketInteractor Interactor;
    }
}
