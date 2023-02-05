using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.XR.Interaction.Toolkit;

public class PlateBehavior : MonoBehaviour
{
    private GameObject _snapSpace;
    private readonly List<SnapSpace> _children = new List<SnapSpace>();
    private AudioSource _audioSource;
    private Receipe[] _receipes;

    // Start is called before the first frame update
    void Start()
    {
        _snapSpace = Resources.Load<GameObject>("Snap Space");
        _audioSource = gameObject.GetComponent<AudioSource>();

        TextAsset textAsset = Resources.Load<TextAsset>("Recipes");
        _receipes = JsonUtility.FromJson<ReceipeBook>(textAsset.text).recipes;

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

        // check for matching crafting recipe
        foreach (Receipe receipe in _receipes.Where(x => x.ingredients.Length == _children.Count)
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
        for (int i = 0; i < _children.Count; i++)
        {
            if (_children[i].Interactor.firstInteractableSelected?.transform.gameObject
                .GetComponent<StackableBehavior>().ingredient == receipe.ingredients[i])
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
